using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactEmailChallenge : UserContactChallenge
  {
    public UserContactEmailChallenge() { }

    public UserContactEmailChallenge(ContactEmailChallengePrepared message)
      : base(message) {
      EmailAddress = message.EmailAddress;
    }
    public string EmailAddress { get; set; }
  }
}