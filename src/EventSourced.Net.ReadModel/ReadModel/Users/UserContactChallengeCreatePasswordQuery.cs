using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactChallengeCreatePasswordQuery : IQuery<Task<UserContactChallengeCreatePasswordView>>
  {
    public Guid CorrelationId { get; }
    public string Token { get; }

    public UserContactChallengeCreatePasswordQuery(Guid correlationId, string token) {
      CorrelationId = correlationId;
      Token = token;
    }
  }

  public class UserContactChallengeCreatePasswordView
  {
    public Guid UserId { get; }
    public string ContactValue { get; }

    internal UserContactChallengeCreatePasswordView(Guid userId, string contactValue) {
      UserId = userId;
      ContactValue = contactValue;
    }
  }

  public class HandleUserContactChallengeCreatePasswordView
    : IHandleQuery<UserContactChallengeCreatePasswordQuery, Task<UserContactChallengeCreatePasswordView>>
  {
    private IExecuteQuery Query { get; }

    public HandleUserContactChallengeCreatePasswordView(IExecuteQuery query) {
      Query = query;
    }

    public async Task<UserContactChallengeCreatePasswordView> Handle(UserContactChallengeCreatePasswordQuery query) {
      UserContactChallengeCreatePasswordView view = null;
      UserDocument user = await Query.Execute(new UserDocumentByContactChallengeCorrelationId(query.CorrelationId));
      var challenge = user?.GetContactChallengeByCorrelationId(query.CorrelationId);
      if (challenge != null && challenge.Token == query.Token)
        view = new UserContactChallengeCreatePasswordView(user.Id, challenge.ContactValue);
      return view;
    }
  }
}
