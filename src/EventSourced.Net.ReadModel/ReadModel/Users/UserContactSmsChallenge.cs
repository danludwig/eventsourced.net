using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactSmsChallenge : UserContactChallenge
  {
    public UserContactSmsChallenge() { }

    public UserContactSmsChallenge(ContactSmsChallengePrepared message)
      : base(message) {
      PhoneNumber = message.PhoneNumber;
      RegionCode = message.RegionCode;
    }

    public ulong PhoneNumber { get; set; }
    public string RegionCode { get; set; }
  }
}