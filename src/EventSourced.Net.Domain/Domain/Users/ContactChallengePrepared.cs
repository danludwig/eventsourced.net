using System;

namespace EventSourced.Net.Domain.Users
{
  public abstract class ContactChallengePrepared : DomainEvent
  {
    protected ContactChallengePrepared(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, ContactChallengePurpose purpose, string stamp, string token)
      : base(aggregateId, happenedOn) {

      CorrelationId = correlationId;
      Purpose = purpose;
      Stamp = stamp;
      Token = token;
    }

    public Guid CorrelationId { get; }
    public ContactChallengePurpose Purpose { get; }
    public string Stamp { get; }
    public string Token { get; }
  }
}