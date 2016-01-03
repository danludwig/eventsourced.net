using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactEmailChallengePrepared : ContactChallengePrepared
  {
    public ContactEmailChallengePrepared(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, string emailAddress, ContactChallengePurpose purpose,
      string stamp, string token, string messageSubject, string messageBody)
      : base(aggregateId, happenedOn, correlationId, purpose, stamp, token) {

      EmailAddress = emailAddress;
      MessageSubject = messageSubject;
      MessageBody = messageBody;
    }

    public string EmailAddress { get; }
    public string MessageSubject { get; }
    public string MessageBody { get; }
  }
}
