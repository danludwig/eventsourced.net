using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace EventSourced.Net.Web.Users.Login
{
  public class ClaimsPrincipalForLogin : IQuery<Task<ClaimsPrincipal>>
  {
    public Guid UserId { get; }
    public string Username { get; }

    public ClaimsPrincipalForLogin(Guid userId, string username) {
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Cannot be empty.", nameof(username));
      UserId = userId;
      Username = username;
    }
  }

  public class HandleClaimsPrincipalForLogin : IHandleQuery<ClaimsPrincipalForLogin, Task<ClaimsPrincipal>>
  {
    private IdentityOptions Options { get; }

    public HandleClaimsPrincipalForLogin(IdentityOptions options) {
      Options = options;
    }

    public Task<ClaimsPrincipal> Handle(ClaimsPrincipalForLogin query) {
      var id = new ClaimsIdentity(Options.Cookies.ApplicationCookieAuthenticationScheme,
          Options.ClaimsIdentity.UserNameClaimType,
          Options.ClaimsIdentity.RoleClaimType);
      id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, query.UserId.ToString()));
      id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, query.Username));
      ClaimsPrincipal principal = new ClaimsPrincipal(id);
      return Task.FromResult(principal);
    }
  }
}
