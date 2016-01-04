using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class VerifyContactChallengeResponse : IHandleCommand<VerifyUserContactChallengeResponse>
  {
    public VerifyContactChallengeResponse(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(VerifyUserContactChallengeResponse message) {
      RepositoryGetResult<User> result = await Repository.TryGetByIdAsync<User>(message.UserId);
      User user = result.Aggregate;
      result.RejectIfNull(nameof(user), message.UserId);
      Exception exceptionToThrowAfterSave;

      user.VerifyContactChallengeResponse(message.CorrelationId, message.Code, out exceptionToThrowAfterSave);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
      if (exceptionToThrowAfterSave != null) throw exceptionToThrowAfterSave;
    }
  }
}
