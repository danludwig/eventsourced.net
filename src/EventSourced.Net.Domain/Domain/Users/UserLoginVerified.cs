using System;

namespace EventSourced.Net.Domain.Users
{
  public class UserLoginVerified : IDomainEvent
  {
    public UserLoginVerified(string login, DateTime happenedOn, string passwordRehash) {
      Login = login;
      HappenedOn = happenedOn;
      PasswordRehash = passwordRehash;
    }

    public string Login { get; }
    public DateTime HappenedOn { get; }
    public string PasswordRehash { get; }
  }
}