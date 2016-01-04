using System;

namespace EventSourced.Net
{
  public class VerifyUserLogin : ICommand
  {
    public VerifyUserLogin(Guid userId, string login, string password) {
      PreValidate(login, password);
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(userId, nameof(userId));
      }

      UserId = userId;
      Login = login?.Trim();
      Password = password;
    }

    public Guid UserId { get; }
    public string Login { get; }
    public string Password { get; }

    public static void PreValidate(string login, string password) {
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(login, nameof(login));
        validate.NotEmpty(password, nameof(password));
      }
    }
  }
}