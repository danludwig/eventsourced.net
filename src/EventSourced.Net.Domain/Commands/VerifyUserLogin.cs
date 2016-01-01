using System;

namespace EventSourced.Net
{
  public class VerifyUserLogin : ICommand
  {
    public VerifyUserLogin(Guid userId, string login, string password) {
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      UserId = userId;
      Login = login?.Trim();
      Password = password;
    }

    public Guid UserId { get; }
    public string Login { get; }
    public string Password { get; }
  }
}