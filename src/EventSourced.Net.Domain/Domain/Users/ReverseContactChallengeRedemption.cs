using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class ReverseContactChallengeRedemption : IHandleCommand<ReverseUserContactChallengeRedemption>
  {
    public ReverseContactChallengeRedemption(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(ReverseUserContactChallengeRedemption message) {
      User user = await Repository.GetByIdAsync<User>(message.UserId);

      user.ReverseContactChallengeRedemption(message.CorrelationId);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
