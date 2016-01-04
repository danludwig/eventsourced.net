using System;

namespace EventSourced.Net
{
  public abstract class DomainEvent : IDomainEvent
  {
    protected DomainEvent(Guid aggregateId, DateTime happenedOn) {
      AggregateId = aggregateId;
      HappenedOn = happenedOn;
    }

    public Guid AggregateId { get; }
    public DateTime HappenedOn { get; }
  }
}
