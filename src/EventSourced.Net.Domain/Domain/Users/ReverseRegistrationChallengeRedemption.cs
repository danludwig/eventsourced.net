using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class ReverseRegistrationChallengeRedemption : IHandleCommand<ReverseUserRegistrationChallengeRedemption>
  {
    public ReverseRegistrationChallengeRedemption(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(ReverseUserRegistrationChallengeRedemption message) {
      User user = await Repository.GetByIdAsync<User>(message.UserId);

      user.ReverseRegistrationChallengeRedemption(message.CorrelationId, message.DuplicateContact, message.DuplicateUsername);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
