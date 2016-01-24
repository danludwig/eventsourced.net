using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactChallengeRedeemView : IQuery<Task<UserContactChallengeRedeemData>>
  {
    public Guid CorrelationId { get; }
    public string Token { get; }

    public UserContactChallengeRedeemView(Guid correlationId, string token) {
      CorrelationId = correlationId;
      Token = token;
    }
  }

  public class UserContactChallengeRedeemData
  {
    public Guid UserId { get; }
    public string ContactValue { get; }
    public ContactChallengePurpose Purpose { get; }

    internal UserContactChallengeRedeemData(Guid userId, string contactValue, ContactChallengePurpose purpose) {
      UserId = userId;
      ContactValue = contactValue;
      Purpose = purpose;
    }
  }

  public class HandleUserContactChallengeRedeemView
    : IHandleQuery<UserContactChallengeRedeemView, Task<UserContactChallengeRedeemData>>
  {
    private IExecuteQuery Query { get; }

    public HandleUserContactChallengeRedeemView(IExecuteQuery query) {
      Query = query;
    }

    public async Task<UserContactChallengeRedeemData> Handle(UserContactChallengeRedeemView query) {
      UserContactChallengeRedeemData view = null;
      UserDocument user = await Query.Execute(new UserDocumentByContactChallengeCorrelationId(query.CorrelationId));
      var challenge = user?.GetContactChallengeByCorrelationId(query.CorrelationId);
      if (challenge != null && challenge.Token == query.Token)
        view = new UserContactChallengeRedeemData(user.Id, challenge.ContactValue, challenge.Purpose);
      return view;
    }
  }
}
