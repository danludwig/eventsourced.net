using System;
using System.Globalization;
using System.Text;

namespace EventSourced.Net.Domain.Users.ContactChallengers
{
  public static class TotpCodeProvider
  {
    public static string Generate(Guid userId, string contactValue, ContactChallengePurpose purpose, string stamp) {
      byte[] stampBytes = Encoding.Unicode.GetBytes(stamp);
      string modifier = $"Totp:{purpose}:{contactValue}:{userId}";
      int code = Rfc6238AuthenticationService.GenerateCode(stampBytes, modifier);
      string totpCode = code.ToString("D6", CultureInfo.InvariantCulture);
      return totpCode;
    }

    public static bool Validate(string totpCode, Guid userId, string contactValue, ContactChallengePurpose purpose, string stamp) {
      byte[] stampBytes = Encoding.Unicode.GetBytes(stamp);
      int code;
      if (!int.TryParse(totpCode, out code))
        return false;
      string modifier = $"Totp:{purpose}:{contactValue}:{userId}";
      bool isValid = Rfc6238AuthenticationService.ValidateCode(stampBytes, code, modifier);
      return isValid;
    }
  }
}
