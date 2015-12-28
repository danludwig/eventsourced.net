using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class HandleVerifyUserContactChallengeResponse : IHandleCommand<VerifyUserContactChallengeResponse>
  {
    public HandleVerifyUserContactChallengeResponse(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(VerifyUserContactChallengeResponse message) {
      var user = await Repository.GetByIdAsync<User>(message.UserId);
      user.VerifyContactChallengeResponse(message.CorrelationId, message.Code);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}