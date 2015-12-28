using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactSmsChallengePrepared : ContactChallengePrepared
  {
    public ContactSmsChallengePrepared(Guid correlationId, Guid challengeId, Guid userId,
      ulong phoneNumber, string regionCode, ContactChallengePurpose purpose,
      string stamp, string code, string token, string message)
      : base(correlationId, challengeId, userId, purpose, stamp, phoneNumber.ToString(), code, token) {

      PhoneNumber = phoneNumber;
      RegionCode = regionCode;
      Message = message;
      }

    public ulong PhoneNumber { get; }
    public string RegionCode { get; }
    public string Message { get; }
  }
}