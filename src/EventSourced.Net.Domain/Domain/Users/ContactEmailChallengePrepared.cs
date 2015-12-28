using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactEmailChallengePrepared : ContactChallengePrepared
  {
    public ContactEmailChallengePrepared(Guid correlationId, Guid userId,
      string emailAddress, ContactChallengePurpose purpose,
      string stamp, string token, string messageSubject, string messageBody)
      : base(correlationId, userId, purpose, stamp, token) {

      EmailAddress = emailAddress;
      MessageSubject = messageSubject;
      MessageBody = messageBody;
    }

    public string EmailAddress { get; }
    public string MessageSubject { get; }
    public string MessageBody { get; }
  }
}
