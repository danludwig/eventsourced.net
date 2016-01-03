using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeResponseMaxInvalidCodesAttempted : DomainEvent
  {
    public ContactChallengeResponseMaxInvalidCodesAttempted(Guid aggregateId, DateTime happenedOn,
      Guid correlationId) : base(aggregateId, happenedOn) {
      CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
  }
}
