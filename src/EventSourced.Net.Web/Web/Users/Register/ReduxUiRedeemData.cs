using EventSourced.Net.ReadModel.Users;
using PhoneNumbers;

namespace EventSourced.Net.Web.Users.Register
{
  public class ReduxUiRedeemData
  {
    public ReduxUiRedeemData(UserContactChallengeTokenData data) {
      ContactValue = data.ContactValue;
      Purpose = data.Purpose;
    }

    public ReduxUiRedeemData(UserContactChallengeRedeemData data) {
      ContactValue = data.ContactValue;
      Purpose = data.Purpose;
    }

    public string ContactValue { get; }
    public ContactChallengePurpose Purpose { get; }

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
