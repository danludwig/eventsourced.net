using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeResponseVerified : DomainEvent
  {
    public ContactChallengeResponseVerified(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, int attemptNumber) : base(aggregateId, happenedOn) {

      CorrelationId = correlationId;
      AttemptNumber = attemptNumber;
    }

    public Guid CorrelationId { get; }
    public int AttemptNumber { get; }
  }
}
