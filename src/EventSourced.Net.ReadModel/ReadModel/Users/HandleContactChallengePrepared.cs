using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class HandleContactChallengePrepared : IHandleEvent<ContactSmsChallengePrepared>, IHandleEvent<ContactEmailChallengePrepared>
  {
    private IPublishEvent EventPublisher { get; }
    private DatabaseSharedSetting DbSettings { get; }

    public HandleContactChallengePrepared(IPublishEvent eventPublisher) {
      EventPublisher = eventPublisher;
      DbSettings = new DatabaseSharedSetting {
        CreateCollectionOnTheFly = true,
        Database = "EventSourced",
        WaitForSync = true,
        Url = "http://localhost:8529",
      };
    }

    public async Task HandleAsync(ContactSmsChallengePrepared message) {
      var challengeItem = new UserContactSmsChallenge {
        Id = message.ChallengeId,
        Purpose = message.Purpose,
        PhoneNumber = message.PhoneNumber,
        RegionCode = message.RegionCode,
      };
      await HandleAsync(message, challengeItem);
    }

    public async Task HandleAsync(ContactEmailChallengePrepared message) {
      var challengeItem = new UserContactEmailChallenge {
        Id = message.ChallengeId,
        Purpose = message.Purpose,
        EmailAddress = message.EmailAddress,
      };
      await HandleAsync(message, challengeItem);
    }

    private async Task HandleAsync(ContactChallengePrepared message, UserContactChallenge challengeItem) {
      using (IArangoDatabase db = new ArangoDatabase(DbSettings)) {
        var user = db.Query<UserDocument>().SingleOrDefault(x => x.Id == message.UserId);
        if (user == null) {
          user = new UserDocument {
            Id = message.UserId,
          };
          await db.InsertAsync<UserDocument>(user);
        }
        if (user.ContactChallengeById(message.ChallengeId) == null) {
          user.AddContactChallenge(challengeItem);
          await db.UpdateAsync<UserDocument>(user);
        }
      }
      await EventPublisher.PublishAsync(new UserContactChallengeViewPrepared(message));
    }
  }
}
