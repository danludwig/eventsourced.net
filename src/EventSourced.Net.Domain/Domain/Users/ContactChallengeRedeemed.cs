using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactChallengeRedeemed : DomainEvent
  {
    public ContactChallengeRedeemed(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, string passwordHash) : base(aggregateId, happenedOn) {

      CorrelationId = correlationId;
      PasswordHash = passwordHash;
    }

    public Guid CorrelationId { get; }
    public string PasswordHash { get; }
  }
}