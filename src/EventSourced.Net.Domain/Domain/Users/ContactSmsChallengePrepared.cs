using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactSmsChallengePrepared : ContactChallengePrepared
  {
    public ContactSmsChallengePrepared(Guid correlationId, Guid challengeId, Guid userId,
      ulong phoneNumber, string regionCode, ContactChallengePurpose purpose,
      string stamp, string code = null, string token = null)
      : base(correlationId, challengeId, userId, purpose, stamp, phoneNumber.ToString(), code, token) {

      PhoneNumber = phoneNumber;
      RegionCode = regionCode;
    }

    public ulong PhoneNumber { get; }
    public string RegionCode { get; }
  }
}