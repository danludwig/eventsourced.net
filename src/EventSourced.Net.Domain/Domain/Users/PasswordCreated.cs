using System;

namespace EventSourced.Net.Domain.Users
{
  public class PasswordCreated : IDomainEvent
  {
    public PasswordCreated(Guid correlationId, Guid userId, string encryptedPassword) {
      CorrelationId = correlationId;
      UserId = userId;
      EncryptedPassword = encryptedPassword;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
    public string EncryptedPassword { get; }
  }
}