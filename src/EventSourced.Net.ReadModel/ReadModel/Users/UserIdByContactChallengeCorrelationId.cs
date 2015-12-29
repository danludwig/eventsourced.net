using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserIdByContactChallengeCorrelationId : IQuery<Task<Guid?>>
  {
    public Guid CorrelationId { get; }

    public UserIdByContactChallengeCorrelationId(Guid correlationId) {
      CorrelationId = correlationId;
    }
  }

  public class HandleUserIdByContactChallengeCorrelationId : IHandleQuery<UserIdByContactChallengeCorrelationId, Task<Guid?>>
  {
    private IProcessQuery Query { get; }

    public HandleUserIdByContactChallengeCorrelationId(IProcessQuery query) {
      Query = query;
    }

    public async Task<Guid?> Handle(UserIdByContactChallengeCorrelationId query) {
      Guid? userId = null;
      UserDocument user = await Query.Execute(new UserDocumentByContactChallengeCorrelationId(query.CorrelationId));
      if (user != null) userId = user.Id;
      return userId;
    }
  }
}
