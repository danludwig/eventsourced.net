using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;

namespace EventSourced.Net.Web.Users.Login
{
  public class LogUserOff : ICommand
  {
    public AuthenticationManager AuthenticationManager { get; }

    public LogUserOff(AuthenticationManager authenticationManager) {

      if (authenticationManager == null) throw new ArgumentNullException(nameof(authenticationManager));
      AuthenticationManager = authenticationManager;
    }
  }

  public class HandleLogUserOff : IHandleCommand<LogUserOff>
  {
    private IdentityOptions Options { get; }

    public HandleLogUserOff(IdentityOptions options) {
      Options = options;
    }

    public async Task HandleAsync(LogUserOff message) {
      await message.AuthenticationManager.SignOutAsync(Options.Cookies.ApplicationCookieAuthenticationScheme);
      await message.AuthenticationManager.SignOutAsync(Options.Cookies.ExternalCookieAuthenticationScheme);
    }
  }
}
