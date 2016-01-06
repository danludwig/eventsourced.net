using System;

namespace EventSourced.Net.Domain.Users
{
  public class RegistrationChallengeRedemptionReversed : DomainEvent
  {
    public RegistrationChallengeRedemptionReversed(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, string duplicateUsername, string duplicateContact) : base(aggregateId, happenedOn)
    {
      CorrelationId = correlationId;
      DuplicateContact = duplicateContact;
      DuplicateUsername = duplicateUsername;
    }

    public Guid CorrelationId { get; }
    public string DuplicateContact { get; }
    public string DuplicateUsername { get; }
  }
}
