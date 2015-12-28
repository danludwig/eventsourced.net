using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactEmailChallengeView : UserContactChallengeView
  {
    public UserContactEmailChallengeView() { }

    public UserContactEmailChallengeView(ContactEmailChallengePrepared message)
      : base(message) {
      EmailAddress = message.EmailAddress;
      MessageSubject = message.MessageSubject;
      MessageBody = message.MessageBody;
    }

    public string EmailAddress { get; set; }
    public string MessageSubject { get; set; }
    public string MessageBody { get; set; }
  }
}