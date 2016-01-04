using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeRedemptionReversed : DomainEvent
  {
    public ContactChallengeRedemptionReversed(Guid aggregateId, DateTime happenedOn,
      Guid correlationId) : base(aggregateId, happenedOn) {

      CorrelationId = correlationId;
      }

    public Guid CorrelationId { get; }
  }
}
