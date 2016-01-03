using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users.Internal.Handlers
{
  public class AddContactChallengeToUserDocument :
    IHandleEvent<ContactSmsChallengePrepared>,
    IHandleEvent<ContactEmailChallengePrepared>
  {
    private IExecuteQuery Query { get; }
    private IArangoDatabase Db { get; }

    public AddContactChallengeToUserDocument(IExecuteQuery query, IArangoDatabase db) {
      Query = query;
      Db = db;
    }

    public async Task HandleAsync(ContactEmailChallengePrepared message) {
      await HandleAsync(message, new UserDocumentContactEmailChallenge(message));
    }

    public async Task HandleAsync(ContactSmsChallengePrepared message) {
      await HandleAsync(message, new UserDocumentContactSmsChallenge(message));
    }

    private async Task HandleAsync(ContactChallengePrepared message, UserDocumentContactChallenge challengeItem) {
      UserDocument user = await Query.Execute(new UserDocumentById(message.AggregateId, throwIfNotFound: true));
      if (user.GetContactChallengeByCorrelationId(message.CorrelationId) == null) {
        user.AddContactChallenge(challengeItem);
        await Db.UpdateAsync<UserDocument>(user);
      }
    }
  }
}
