using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNet.DataProtection;

namespace EventSourced.Net.Domain.Users.ContactChallengers
{
  public class DataProtectionTokenProvider
  {
    private static Func<IDataProtectionProvider> ProviderFactory { get; set; }
    private static readonly TimeSpan TokenLifespan = TimeSpan.FromMinutes(30);
    private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

    public static void SetProviderFactory(Func<IDataProtectionProvider> providerFactory) {
      if (providerFactory == null) throw new ArgumentNullException(nameof(providerFactory));
      ProviderFactory = providerFactory;
    }

    internal static string Generate(Guid userId, ContactChallengePurpose purpose, string stamp) {
      return new DataProtectionTokenProvider().GenerateCore(userId, purpose, stamp);
    }

    internal static bool Validate(string token, Guid userId, ContactChallengePurpose purpose, string stamp) {
      return new DataProtectionTokenProvider().ValidateCore(token, userId, purpose, stamp);
    }

    private IDataProtector Protector { get; }

    private DataProtectionTokenProvider() {
      IDataProtectionProvider provider = ProviderFactory();
      string[] purposes = Enum.GetValues(typeof(ContactChallengePurpose))
       .Cast<ContactChallengePurpose>()
       .Except(new[] { ContactChallengePurpose.Invalid })
       .Select(x => x.ToString())
       .ToArray();
      Protector = provider.CreateProtector(GetType().Assembly.GetName().Name, purposes);
    }

    private string GenerateCore(Guid userId, ContactChallengePurpose purpose, string stamp) {
      MemoryStream memoryStream = new MemoryStream();
      using (var writer = new BinaryWriter(memoryStream, DefaultEncoding, true)) {
        writer.Write(DateTimeOffset.UtcNow.Ticks);
        writer.Write(userId.ToString());
        writer.Write(purpose.ToString());
        writer.Write(stamp);
      }

      byte[] protectedBytes = Protector.Protect(memoryStream.ToArray());
      string token = Convert.ToBase64String(protectedBytes);
      return token;
    }

    private bool ValidateCore(string token, Guid userId, ContactChallengePurpose purpose, string stamp) {
      try {
        byte[] unprotectedData = Protector.Unprotect(Convert.FromBase64String(token));
        var ms = new MemoryStream(unprotectedData);
        using (var reader = new BinaryReader(ms, DefaultEncoding, true)) {
          DateTimeOffset creationTime = new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
          DateTimeOffset expirationTime = creationTime + TokenLifespan;
          if (expirationTime < DateTimeOffset.UtcNow) {
            return false;
          }

          string tokenUserId = reader.ReadString();
          if (!string.Equals(tokenUserId, userId.ToString(), StringComparison.OrdinalIgnoreCase)) {
            return false;
          }

          var tokenPurpose = reader.ReadString();
          if (!string.Equals(tokenPurpose, purpose.ToString())) {
            return false;
          }

          var tokenStamp = reader.ReadString();
          if (reader.PeekChar() != -1) {
            return false;
          }

          bool isTokenValid = tokenStamp == stamp;
          return isTokenValid;
        }
      } catch {
        // do not leak exception
        return false;
      }
    }
  }
}
