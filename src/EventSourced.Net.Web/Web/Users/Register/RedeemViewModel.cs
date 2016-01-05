using EventSourced.Net.Domain.Users;
using PhoneNumbers;

namespace EventSourced.Net.Web.Users.Register
{
  public class RedeemViewModel
  {
    public ShortGuid CorrelationId { get; set; }
    public string Token { get; set; }
    public string ContactValue { get; set; }
    public ContactChallengePurpose Purpose { get; set; }

    public string PhoneNumberFormatted
    {
      get
      {
        PhoneNumber phoneNumber = ContactIdParser.AsPhoneNumber(ContactValue);
        if (phoneNumber == null) return null;

        PhoneNumberUtil util = PhoneNumberUtil.GetInstance();
        string formatted = util.Format(phoneNumber, PhoneNumberFormat.NATIONAL);
        return formatted;
      }
    }
  }
}