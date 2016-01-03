using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeResponseInvalidCodeAttempted : IDomainEvent
  {
    public ContactChallengeResponseInvalidCodeAttempted(Guid correlationId, Guid userId, string code, int attemptNumber, DateTime happenedOn) {
      CorrelationId = correlationId;
      UserId = userId;
      Code = code;
      AttemptNumber = attemptNumber;
      HappenedOn = happenedOn;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
    public string Code { get; }
    public int AttemptNumber { get; }
    public DateTime HappenedOn { get; }
  }
}
