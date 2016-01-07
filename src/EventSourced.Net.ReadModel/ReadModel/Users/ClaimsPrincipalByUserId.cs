using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace EventSourced.Net.ReadModel.Users
{
  public class ClaimsPrincipalByUserId : IQuery<Task<ClaimsPrincipal>>
  {
    public Guid UserId { get; }

    public ClaimsPrincipalByUserId(Guid userId) {
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      UserId = userId;
    }
  }

  public class HandleClaimsPrincipalByUserId : IHandleQuery<ClaimsPrincipalByUserId, Task<ClaimsPrincipal>>
  {
    private IExecuteQuery Query { get; }
    private IdentityOptions Options { get; }

    public HandleClaimsPrincipalByUserId(IExecuteQuery query, IdentityOptions options)
    {
      Query = query;
      Options = options;
    }

    public async Task<ClaimsPrincipal> Handle(ClaimsPrincipalByUserId query) {
      var id = new ClaimsIdentity(Options.Cookies.ApplicationCookieAuthenticationScheme,
        ClaimTypes.Name, ClaimTypes.Role);
      var claims = await Query.Execute(new ClaimsByUserId(query.UserId));
      foreach (Claim claim in claims) {
        id.AddClaim(claim);
      }
      ClaimsPrincipal principal = new ClaimsPrincipal(id);
      return principal;
    }
  }
}
