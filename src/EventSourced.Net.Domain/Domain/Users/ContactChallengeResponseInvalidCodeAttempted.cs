using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeResponseInvalidCodeAttempted : DomainEvent
  {
    public ContactChallengeResponseInvalidCodeAttempted(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, string code, int attemptNumber) : base(aggregateId, happenedOn) {

      CorrelationId = correlationId;
      Code = code;
      AttemptNumber = attemptNumber;
    }

    public Guid CorrelationId { get; }
    public string Code { get; }
    public int AttemptNumber { get; }
  }
}
