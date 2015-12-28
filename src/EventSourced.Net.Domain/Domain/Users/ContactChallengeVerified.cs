using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeVerified : IDomainEvent
  {
    public ContactChallengeVerified(Guid correlationId, Guid userId) {
      CorrelationId = correlationId;
      UserId = userId;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
  }
}