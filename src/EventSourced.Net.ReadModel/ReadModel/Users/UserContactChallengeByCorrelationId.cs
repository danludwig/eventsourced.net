using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactChallengeByCorrelationId : IQuery<Task<UserContactChallengeView>>
  {
    public Guid CorrelationId { get; }

    public UserContactChallengeByCorrelationId(Guid correlationId) {
      CorrelationId = correlationId;
    }
  }

  public class HandleUserContactChallengeByCorrelationId : IHandleQuery<UserContactChallengeByCorrelationId, Task<UserContactChallengeView>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserContactChallengeByCorrelationId(IArangoDatabase db) {
      Db = db;
    }

    public Task<UserContactChallengeView> Handle(UserContactChallengeByCorrelationId query) {
      UserContactChallengeView result = null;
      Guid userId = Db.Query<UserContactChallengeView>()
        .Where(x => x.CorrelationId == query.CorrelationId)
        .Select(x => x.UserId).SingleOrDefault();
      if (userId != default(Guid)) {
        UserView user = Db.Query<UserView>().SingleOrDefault(x => x.Id == userId);
        if (user != null) {
          result = user.ContactChallenges.SingleOrDefault(x => x.CorrelationId == query.CorrelationId);
        }
      }
      return Task.FromResult(result);
    }
  }
}
