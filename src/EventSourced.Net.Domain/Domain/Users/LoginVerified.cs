using System;

namespace EventSourced.Net.Domain.Users
{
  public class LoginVerified : IDomainEvent
  {
    public LoginVerified(Guid userId, string login, DateTime happenedOn, string passwordRehash) {
      UserId = userId;
      Login = login;
      HappenedOn = happenedOn;
      PasswordRehash = passwordRehash;
    }

    public Guid UserId { get; }
    public string Login { get; }
    public DateTime HappenedOn { get; }
    public string PasswordRehash { get; }
  }
}