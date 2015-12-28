using System;

namespace EventSourced.Net.Domain.Users
{
  public abstract class ContactChallengePrepared : IDomainEvent
  {
    protected ContactChallengePrepared(Guid correlationId, Guid userId,
      ContactChallengePurpose purpose, string stamp, string token) {

      CorrelationId = correlationId;
      UserId = userId;
      Purpose = purpose;
      Stamp = stamp;
      Token = token;
    }

    public Guid CorrelationId { get; }
    public Guid UserId { get; }
    public ContactChallengePurpose Purpose { get; }
    public string Stamp { get; }
    public string Token { get; }
  }
}