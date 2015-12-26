using System;
using System.Net.Mail;

namespace EventSourced.Net.Domain.Users
{
  public class ContactEmailChallengePrepared : ContactChallengePrepared
  {
    public ContactEmailChallengePrepared(Guid challengeId, Guid userId,
      MailAddress mailAddress, ContactChallengePurpose purpose,
      string stamp, string code = null, string token = null)
      : base(challengeId, userId, purpose, stamp, mailAddress.Address, code, token) {

      EmailAddress = mailAddress.Address;
    }

    public string EmailAddress { get; }
  }
}
