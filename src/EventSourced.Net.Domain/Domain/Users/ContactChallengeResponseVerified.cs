using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeResponseVerified : IDomainEvent
  {
    public ContactChallengeResponseVerified(Guid correlationId, Guid userId, int attemptNumber, DateTime happenedOn) {
      CorrelationId = correlationId;
      UserId = userId;
      AttemptNumber = attemptNumber;
      HappenedOn = happenedOn;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
    public int AttemptNumber { get; }
    public DateTime HappenedOn { get; }
  }
}
