using System;

namespace EventSourced.Net.ReadModel.Users.Internal.Events
{
  public class ContactChallengeRedemptionConcluded : IEvent
  {
    public ContactChallengeRedemptionConcluded(Guid userId, Guid correlationId) {
      UserId = userId;
      CorrelationId = correlationId;
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
  }
}
