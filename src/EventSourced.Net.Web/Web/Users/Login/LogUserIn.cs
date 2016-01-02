using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;

namespace EventSourced.Net.Web.Users.Login
{
  public class LogUserIn : ICommand
  {
    public Guid UserId { get; }
    public string Username { get; }
    public AuthenticationManager AuthenticationManager { get; }
    public AuthenticationProperties AuthenticationProperties { get; }

    public LogUserIn(Guid userId, string username,
      AuthenticationManager authenticationManager,
      AuthenticationProperties authenticationProperties = null) {

      if (authenticationManager == null) throw new ArgumentNullException(nameof(authenticationManager));
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Cannot be empty.", nameof(username));
      UserId = userId;
      Username = username;
      AuthenticationManager = authenticationManager;
      AuthenticationProperties = authenticationProperties ?? new AuthenticationProperties();
    }
  }

  public class HandleLogUserIn : IHandleCommand<LogUserIn>
  {
    private IExecuteQuery Query { get; }
    private IdentityOptions Options { get; }

    public HandleLogUserIn(IExecuteQuery query, IdentityOptions options) {
      Query = query;
      Options = options;
    }

    public async Task HandleAsync(LogUserIn message) {
      ClaimsPrincipal principal = await Query
        .Execute(new ClaimsPrincipalForLogin(message.UserId, message.Username));
      await message.AuthenticationManager
        .SignInAsync(Options.Cookies.ApplicationCookieAuthenticationScheme,
          principal, message.AuthenticationProperties);
    }
  }
}
