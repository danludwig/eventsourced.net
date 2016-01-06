using System;

namespace EventSourced.Net.Domain.Users
{
  public class RegistrationChallengeRedeemed : DomainEvent
  {
    public RegistrationChallengeRedeemed(Guid aggregateId, DateTime happenedOn, Guid correlationId,
      string username, string passwordHash) : base(aggregateId, happenedOn) {

      CorrelationId = correlationId;
      Username = username;
      PasswordHash = passwordHash;
    }

    public Guid CorrelationId { get; }
    public string Username { get; }
    public string NormalizedUsername => Username?.ToLower();
    public string PasswordHash { get; }
  }
}