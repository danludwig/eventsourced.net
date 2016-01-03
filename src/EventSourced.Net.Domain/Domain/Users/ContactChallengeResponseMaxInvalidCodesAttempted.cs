using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeResponseMaxInvalidCodesAttempted : IDomainEvent
  {
    public ContactChallengeResponseMaxInvalidCodesAttempted(Guid correlationId, Guid userId, DateTime happenedOn) {
      CorrelationId = correlationId;
      UserId = userId;
      HappenedOn = happenedOn;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
    public DateTime HappenedOn { get; }
  }
}
