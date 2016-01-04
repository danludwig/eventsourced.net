using System;

namespace EventSourced.Net.Domain
{
  public abstract class AggregateConstructed : DomainEvent
  {
    protected AggregateConstructed(Guid aggregateId, DateTime happenedOn)
      : base(aggregateId, happenedOn) { }
  }
}
