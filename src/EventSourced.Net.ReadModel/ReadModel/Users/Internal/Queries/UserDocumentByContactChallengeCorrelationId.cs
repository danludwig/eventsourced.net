using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.ReadModel.Users.Internal.Documents;

namespace EventSourced.Net.ReadModel.Users.Internal.Queries
{
  public class UserDocumentByContactChallengeCorrelationId : IQuery<Task<UserDocument>>
  {
    public Guid CorrelationId { get; }

    internal UserDocumentByContactChallengeCorrelationId(Guid correlationId) {
      CorrelationId = correlationId;
    }
  }

  public class HandleUserDocumentByContactChallengeCorrelationId : IHandleQuery<UserDocumentByContactChallengeCorrelationId, Task<UserDocument>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserDocumentByContactChallengeCorrelationId(IArangoDatabase db) {
      Db = db;
    }

    public Task<UserDocument> Handle(UserDocumentByContactChallengeCorrelationId query) {
      UserDocument user = Db.Query<UserDocument>()
        .SingleOrDefault(x =>
          AQL.In(query.CorrelationId, x.ContactEmailChallenges.Select(y => y.CorrelationId)) ||
          AQL.In(query.CorrelationId, x.ContactSmsChallenges.Select(y => y.CorrelationId)));
      return Task.FromResult(user);
    }
  }
}
