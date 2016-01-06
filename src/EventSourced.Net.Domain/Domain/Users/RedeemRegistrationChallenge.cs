using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class RedeemRegistrationChallenge : IHandleCommand<RedeemUserRegistrationChallenge>
  {
    public RedeemRegistrationChallenge(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(RedeemUserRegistrationChallenge message) {
      RepositoryGetResult<User> result = await Repository.TryGetByIdAsync<User>(message.UserId);
      User user = result.Aggregate;
      result.RejectIfNull(nameof(user), message.UserId);

      user.RedeemRegistrationChallenge(message.CorrelationId, message.Token, message.Username, message.Password);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
