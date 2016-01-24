using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactChallengeTokenView : IQuery<Task<UserContactChallengeTokenData>>
  {
    public Guid CorrelationId { get; }

    public UserContactChallengeTokenView(Guid correlationId) {
      CorrelationId = correlationId;
    }
  }

  public class UserContactChallengeTokenData : UserContactChallengeRedeemData
  {
    public string Token { get; }

    internal UserContactChallengeTokenData(Guid userId, string token, string contactValue, ContactChallengePurpose purpose)
      : base(userId, contactValue, purpose) {
      Token = token;
    }
  }

  public class HandleUserContactChallengeTokenView
    : IHandleQuery<UserContactChallengeTokenView, Task<UserContactChallengeTokenData>>
  {
    private IExecuteQuery Query { get; }

    public HandleUserContactChallengeTokenView(IExecuteQuery query) {
      Query = query;
    }

    public async Task<UserContactChallengeTokenData> Handle(UserContactChallengeTokenView view) {
      UserContactChallengeTokenData data = null;
      UserDocument user = await Query.Execute(new UserDocumentByContactChallengeCorrelationId(view.CorrelationId));
      var challenge = user?.GetContactChallengeByCorrelationId(view.CorrelationId);
      if (challenge != null)
        data = new UserContactChallengeTokenData(user.Id, challenge.Token, challenge.ContactValue, challenge.Purpose);
      return data;
    }
  }
}
