using System;

namespace EventSourced.Net.Domain.Users
{
  public class LoginInvalidPasswordAttempted : IDomainEvent
  {
    public LoginInvalidPasswordAttempted(Guid userId, string login, DateTime happenedOn) {
      UserId = userId;
      Login = login;
      HappenedOn = happenedOn;
    }

    public Guid UserId { get; }
    public string Login { get; }
    public DateTime HappenedOn { get; }
  }
}