using System;
using System.Net.Mail;

namespace EventSourced.Net.Domain.Users
{
  public class ContactEmailChallengePrepared : ContactChallengePrepared
  {
    public ContactEmailChallengePrepared(Guid challengeId, Guid userId,
      string emailAddress, ContactChallengePurpose purpose,
      string stamp, string code = null, string token = null)
      : base(challengeId, userId, purpose, stamp, emailAddress, code, token) {

      EmailAddress = emailAddress;
    }

    public string EmailAddress { get; }
  }
}
