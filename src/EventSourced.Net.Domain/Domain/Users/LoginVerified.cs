using System;

namespace EventSourced.Net.Domain.Users
{
  public class LoginVerified : DomainEvent
  {
    public LoginVerified(Guid aggregateId, DateTime happenedOn,
      string login, string passwordRehash) : base(aggregateId, happenedOn) {

      Login = login;
      PasswordRehash = passwordRehash;
    }

    public string Login { get; }
    public string PasswordRehash { get; }
  }
}