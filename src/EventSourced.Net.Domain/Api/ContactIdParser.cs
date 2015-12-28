using System;
using System.Net.Mail;
using PhoneNumbers;

namespace EventSourced.Net
{
  public static class ContactIdParser
  {
    public const string DefaultRegionCode = "US";

    public static MailAddress AsMailAddress(string text) {
      try {
        return new MailAddress(text);
      } catch (FormatException) {
        return null;
      }
    }

    public static PhoneNumber AsPhoneNumber(string text, string regionCode = DefaultRegionCode) {
      var phoneNumberUtil = PhoneNumberUtil.GetInstance();
      try {
        PhoneNumber phoneNumber = phoneNumberUtil.Parse(text, regionCode);
        bool isValid = phoneNumberUtil.IsValidNumber(phoneNumber);
        return isValid ? phoneNumber : null;
      } catch (NumberParseException) {
        return null;
      }
    }

    public static PhoneNumber AsPhoneNumber(ulong number, string regionCode = DefaultRegionCode) {
      return AsPhoneNumber(number.ToString(), regionCode);
    }

    public static string Normalize(string text) {
      string normalizedText = text?.Trim().ToLower();
      var phoneNumber = AsPhoneNumber(text);
      if (phoneNumber != null) {
        normalizedText = phoneNumber.NationalNumber.ToString().ToLower();
      }
      return normalizedText;
    }
  }
}
