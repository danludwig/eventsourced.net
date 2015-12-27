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
    private IPublishEvent EventPublisher { get; }

    public HandleContactChallengePrepared(IProcessQuery query, IArangoDatabase db, IPublishEvent eventPublisher) {
      Query = query;
      Db = db;
      EventPublisher = eventPublisher;
    }

    public async Task HandleAsync(ContactSmsChallengePrepared message) {
      await HandleAsync(message, new UserContactSmsChallenge(message));
    }

    public async Task HandleAsync(ContactEmailChallengePrepared message) {
      await HandleAsync(message, new UserContactEmailChallenge(message));
    }

    private async Task HandleAsync(ContactChallengePrepared message, UserContactChallenge challengeItem) {
      UserDocument user = await Query.Execute(new UserDocumentById(message.UserId));
      if (user == null) {
        user = new UserDocument { Id = message.UserId, };
        await Db.InsertAsync<UserDocument>(user);
      }
      if (user.ContactChallengeById(message.ChallengeId) == null) {
        user.AddContactChallenge(challengeItem);
        await Db.UpdateAsync<UserDocument>(user);
      }
      await EventPublisher.PublishAsync(new UserContactChallengeViewPrepared(message));
    }
  }
}
