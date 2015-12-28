using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactSmsChallengeView : UserContactChallengeView
  {
    public UserContactSmsChallengeView() { }

    public UserContactSmsChallengeView(ContactSmsChallengePrepared message)
      : base(message) {
      PhoneNumber = message.PhoneNumber;
      RegionCode = message.RegionCode;
    }

    public ulong PhoneNumber { get; set; }
    public string RegionCode { get; set; }
  }
}