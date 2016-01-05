using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;

namespace EventSourced.Net.Web.Users.Login
{
  public class LogUserIn : ICommand
  {
    public string Login { get; }
    public string Password { get; }
    public AuthenticationManager AuthenticationManager { get; }
    public AuthenticationProperties AuthenticationProperties { get; }

    public LogUserIn(string login, string password, AuthenticationManager authenticationManager, AuthenticationProperties authenticationProperties = null) {
      VerifyUserLogin.PreValidate(login, password);
      using (var validate = new CommandValidator()) {
        validate.NotNull(authenticationManager, nameof(authenticationManager));
      }

      Login = login;
      Password = password;
      AuthenticationManager = authenticationManager;
      AuthenticationProperties = authenticationProperties ?? new AuthenticationProperties();
    }
  }

  public class HandleLogUserIn : IHandleCommand<LogUserIn>
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }
    private IdentityOptions Options { get; }

    public HandleLogUserIn(ISendCommand command, IExecuteQuery query, IdentityOptions options) {
      Command = command;
      Query = query;
      Options = options;
    }

    public async Task HandleAsync(LogUserIn message) {
      Guid? userId = await Query.Execute(new UserIdByLogin(message.Login));
      if (!userId.HasValue)
        throw new CommandRejectedException(nameof(message.Login), null, CommandRejectionReason.Unverified);

      await Command.SendAsync(new VerifyUserLogin(userId.Value, message.Login, message.Password));

      ClaimsPrincipal principal = await Query
        .Execute(new ClaimsPrincipalForLogin(userId.Value, message.Login));
      await message.AuthenticationManager
        .SignInAsync(Options.Cookies.ApplicationCookieAuthenticationScheme,
          principal, message.AuthenticationProperties);
    }
  }
}
