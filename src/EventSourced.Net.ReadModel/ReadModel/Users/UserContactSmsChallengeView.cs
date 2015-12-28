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
      Message = message.Message;
    }

    public ulong PhoneNumber { get; set; }
    public string RegionCode { get; set; }
    public string Message { get; set; }
    public override string ContactValue => PhoneNumber.ToString();
  }
}