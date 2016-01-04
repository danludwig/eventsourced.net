using System;

namespace EventSourced.Net.Domain.Users
{
  public class Created : AggregateConstructed
  {
    public Created(Guid aggregateId, DateTime happenedOn) : base(aggregateId, happenedOn) { }
  }
}
