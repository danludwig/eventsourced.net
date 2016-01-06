using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class PrepareRegistrationChallenge : IHandleCommand<PrepareUserRegistrationChallenge>
  {
    public PrepareRegistrationChallenge(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(PrepareUserRegistrationChallenge message) {
      var user = new User(GuidComb.NewGuid());
      user.PrepareRegistrationChallenge(message.CorrelationId, message.EmailOrPhone, message.Purpose);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
