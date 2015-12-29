using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public class UserDocumentContactSmsChallenge : UserDocumentContactChallenge
  {
    public UserDocumentContactSmsChallenge() { }

    public UserDocumentContactSmsChallenge(ContactSmsChallengePrepared message)
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