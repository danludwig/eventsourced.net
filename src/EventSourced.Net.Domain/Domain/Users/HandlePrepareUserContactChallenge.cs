using System;
using System.Collections.Generic;
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
      var user = new User();
      user.PrepareContactIdChallenge(message.CorrelationId, message.EmailOrPhone);

      var commitId = Guid.NewGuid();
      Action<IDictionary<string, object>> updateMetadata = x => {
        x.Add(nameof(message.CorrelationId), message.CorrelationId);
      };
      await Repository.SaveAsync(user, commitId, updateMetadata);
    }
  }
}
