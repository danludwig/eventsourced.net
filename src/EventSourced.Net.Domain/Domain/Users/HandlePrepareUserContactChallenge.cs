using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class HandlePrepareUserContactChallenge : IHandleCommand<PrepareUserContactChallenge>
  {
    public HandlePrepareUserContactChallenge(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(PrepareUserContactChallenge message) {
      var user = new User(GuidComb.NewGuid());
      user.PrepareContactChallenge(message.CorrelationId, message.EmailOrPhone);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
