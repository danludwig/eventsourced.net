using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactEmailChallengePrepared : ContactChallengePrepared
  {
    public ContactEmailChallengePrepared(Guid correlationId, Guid challengeId, Guid userId,
      string emailAddress, ContactChallengePurpose purpose,
      string stamp, string code, string token, string messageSubject, string messageBody)
      : base(correlationId, challengeId, userId, purpose, stamp, emailAddress, code, token) {

      EmailAddress = emailAddress;
      MessageSubject = messageSubject;
      MessageBody = messageBody;
    }

    public string EmailAddress { get; }
    public string MessageSubject { get; }
    public string MessageBody { get; }
  }
}
