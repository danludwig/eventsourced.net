using System;

namespace EventSourced.Net
{
  public abstract class DomainEvent : Event, IDomainEvent
  {
    protected DomainEvent(Guid aggregateId, DateTime happenedOn) : base(happenedOn) {
      AggregateId = aggregateId;
    }

    public Guid AggregateId { get; }
  }
}
