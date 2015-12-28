using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactEmailChallengeView : UserContactChallengeView
  {
    public UserContactEmailChallengeView() { }

    public UserContactEmailChallengeView(ContactEmailChallengePrepared message)
      : base(message) {
      EmailAddress = message.EmailAddress;
    }
    public string EmailAddress { get; set; }
  }
}