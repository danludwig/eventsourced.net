using System;

namespace EventSourced.Net.Domain.Users
{
  public class LoginInvalidPasswordAttempted : DomainEvent
  {
    public LoginInvalidPasswordAttempted(Guid aggregateId, DateTime happenedOn,
      string login) : base(aggregateId, happenedOn) {
      Login = login;
    }

    public string Login { get; }
  }
}