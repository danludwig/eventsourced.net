using System;
using PhoneNumbers;

namespace EventSourced.Net.Domain.Users
{
  public class ContactSmsChallengePrepared : ContactChallengePrepared
  {
    public ContactSmsChallengePrepared(Guid challengeId, Guid userId,
      PhoneNumber phoneNumber, string regionCode, ContactChallengePurpose purpose,
      string stamp, string code = null, string token = null)
      : base(challengeId, userId, purpose, stamp, phoneNumber.NationalNumber.ToString(), code, token) {

      PhoneNumber = phoneNumber.NationalNumber;
      RegionCode = regionCode;
    }

    public ulong PhoneNumber { get; }
    public string RegionCode { get; }
  }
}