using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class HandleContactChallengePrepared :
    IHandleEvent<ContactSmsChallengePrepared>,
    IHandleEvent<ContactEmailChallengePrepared>
  {
    private IProcessQuery Query { get; }
    private IArangoDatabase Db { get; }

    public HandleContactChallengePrepared(IProcessQuery query, IArangoDatabase db) {
      Query = query;
      Db = db;
    }

    public async Task HandleAsync(ContactEmailChallengePrepared message) {
      await HandleAsync(message, new UserContactEmailChallengeView(message));
    }

    public async Task HandleAsync(ContactSmsChallengePrepared message) {
      await HandleAsync(message, new UserContactSmsChallengeView(message));
    }

    private async Task HandleAsync(ContactChallengePrepared message, UserContactChallengeView challengeItem) {
      UserView user = await Query.Execute(new UserById(message.UserId));
      if (user == null) {
        user = new UserView { Id = message.UserId, };
        await Db.InsertAsync<UserView>(user);
      }
      if (user.ContactChallengeByCorrelationId(message.CorrelationId) == null) {
        user.AddContactChallenge(challengeItem);
        await Db.UpdateAsync<UserView>(user);
        Db.Insert<UserContactChallengeView>(challengeItem);
      }
    }
  }
}
