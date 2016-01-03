using System;

namespace EventSourced.Net.Domain.Users
{
  public class UserCreated : DomainEvent
  {
    public UserCreated(Guid aggregateId, DateTime happenedOn)
      : base(aggregateId, happenedOn) { }
  }
}
