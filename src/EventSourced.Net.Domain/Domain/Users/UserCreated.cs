using System;

namespace EventSourced.Net.Domain.Users
{
  public class UserCreated : IDomainEvent
  {
    public Guid Id { get; }

    public UserCreated(Guid id) {
      Id = id;
    }
  }
}