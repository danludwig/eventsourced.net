using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class RedeemContactChallenge : IHandleCommand<RedeemUserContactChallenge>
  {
    public RedeemContactChallenge(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(RedeemUserContactChallenge message) {
      RepositoryGetResult<User> result = await Repository.TryGetByIdAsync<User>(message.UserId);
      User user = result.Aggregate;
      result.RejectIfNull(nameof(user), message.UserId);

      user.RedeemContactChallenge(message.CorrelationId, message.Token, message.Password);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
