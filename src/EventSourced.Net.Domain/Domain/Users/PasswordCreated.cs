using System;

namespace EventSourced.Net.Domain.Users
{
  public class PasswordCreated : IDomainEvent
  {
    public PasswordCreated(Guid correlationId, Guid userId, string passwordHash) {
      CorrelationId = correlationId;
      UserId = userId;
      PasswordHash = passwordHash;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
    public string PasswordHash { get; }
  }
}