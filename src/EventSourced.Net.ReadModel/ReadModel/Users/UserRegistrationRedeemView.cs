using System;
using System.Threading.Tasks;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserRegistrationRedeemView : IQuery<Task<UserRegistrationRedeemData>>
  {
    public Guid CorrelationId { get; }
    public string Token { get; }

    public UserRegistrationRedeemView(Guid correlationId, string token) {
      CorrelationId = correlationId;
      Token = token;
    }
  }

  public class UserRegistrationRedeemData
  {
    public Guid UserId { get; }
    public string ContactValue { get; }
    public ContactChallengePurpose Purpose { get; }

    internal UserRegistrationRedeemData(Guid userId, string contactValue, ContactChallengePurpose purpose) {
      UserId = userId;
      ContactValue = contactValue;
      Purpose = purpose;
    }
  }

  public class HandleUserRegistrationRedeemView
    : IHandleQuery<UserRegistrationRedeemView, Task<UserRegistrationRedeemData>>
  {
    private IExecuteQuery Query { get; }

    public HandleUserRegistrationRedeemView(IExecuteQuery query) {
      Query = query;
    }

    public async Task<UserRegistrationRedeemData> Handle(UserRegistrationRedeemView query) {
      UserRegistrationRedeemData view = null;
      UserDocument user = await Query.Execute(new UserDocumentByContactChallengeCorrelationId(query.CorrelationId));
      var challenge = user?.GetContactChallengeByCorrelationId(query.CorrelationId);
      if (challenge != null && challenge.Token == query.Token)
        view = new UserRegistrationRedeemData(user.Id, challenge.ContactValue, challenge.Purpose);
      return view;
    }
  }
}
