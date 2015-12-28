using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactEmailChallengePrepared : ContactChallengePrepared
  {
    public ContactEmailChallengePrepared(Guid correlationId, Guid challengeId, Guid userId,
      string emailAddress, ContactChallengePurpose purpose,
      string stamp, string code = null, string token = null)
      : base(correlationId, challengeId, userId, purpose, stamp, emailAddress, code, token) {

      EmailAddress = emailAddress;
    }

    public string EmailAddress { get; }
  }
}
