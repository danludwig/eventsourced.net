using System;

namespace EventSourced.Net.Domain.Users
{
  public class UserAttemptedLoginWithUnverifiedPassword : IDomainEvent
  {
    public UserAttemptedLoginWithUnverifiedPassword(string login, DateTime happenedOn) {
      Login = login;
      HappenedOn = happenedOn;
    }

    public string Login { get; }
    public DateTime HappenedOn { get; }
  }
}