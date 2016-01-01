using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactChallengeTokenQuery : IQuery<Task<UserContactChallengeTokenView>>
  {
    public Guid CorrelationId { get; }

    public UserContactChallengeTokenQuery(Guid correlationId) {
      CorrelationId = correlationId;
    }
  }

  public class UserContactChallengeTokenView
  {
    public Guid UserId { get; }
    public string Token { get; }

    internal UserContactChallengeTokenView(Guid userId, string token) {
      UserId = userId;
      Token = token;
    }
  }

  public class HandleUserContactChallengeTokenQuery
    : IHandleQuery<UserContactChallengeTokenQuery, Task<UserContactChallengeTokenView>>
  {
    private IExecuteQuery Query { get; }

    public HandleUserContactChallengeTokenQuery(IExecuteQuery query) {
      Query = query;
    }

    public async Task<UserContactChallengeTokenView> Handle(UserContactChallengeTokenQuery query) {
      UserContactChallengeTokenView view = null;
      UserDocument user = await Query.Execute(new UserDocumentByContactChallengeCorrelationId(query.CorrelationId));
      var challenge = user?.GetContactChallengeByCorrelationId(query.CorrelationId);
      if (challenge != null)
        view = new UserContactChallengeTokenView(user.Id, challenge.Token);
      return view;
    }
  }
}
