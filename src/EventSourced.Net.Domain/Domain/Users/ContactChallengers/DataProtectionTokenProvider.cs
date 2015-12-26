// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EventSourced.Net.Domain.Users.ContactChallengers
{
  internal class DataProtectionTokenProvider
  {
    internal static readonly DataProtectionTokenProvider Instance = new DataProtectionTokenProvider();
    private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
    private DpapiDataProtector Protector { get; }
    private TimeSpan TokenLifespan { get; }

    private DataProtectionTokenProvider() {
      ContactChallengePurpose[] purposes = Enum.GetValues(typeof(ContactChallengePurpose))
        .Cast<ContactChallengePurpose>()
        .Except(new[] { ContactChallengePurpose.Invalid })
        .ToArray();
      string primaryPurpose = purposes.First().ToString();
      string[] specificPurposes = purposes.Skip(1).Select(x => x.ToString()).ToArray();
      Protector = new DpapiDataProtector("EventSourced.Net", primaryPurpose, specificPurposes) {
        Scope = DataProtectionScope.LocalMachine,
      };
      TokenLifespan = TimeSpan.FromMinutes(30);
    }

    internal string Generate(Guid userId, ContactChallengePurpose purpose, string stamp) {
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

    internal bool Validate(string token, Guid userId, ContactChallengePurpose purpose, string stamp) {
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
