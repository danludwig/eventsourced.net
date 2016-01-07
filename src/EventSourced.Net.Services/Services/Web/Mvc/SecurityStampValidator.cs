using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.OptionsModel;
using SimpleInjector;

namespace EventSourced.Net.Services.Web.Mvc
{
  public class SecurityStampValidator : ISecurityStampValidator
  {
    private IExecuteQuery Query { get; }
    private IdentityOptions Options { get; }

    public SecurityStampValidator(Container container, IOptions<IdentityOptions> options) {
      Query = container.GetInstance<IExecuteQuery>();
      Options = options.Value;
    }

    public async Task ValidateAsync(CookieValidatePrincipalContext context) {
      string userId = context.Principal.GetUserId();
      var principal = await ValidateSecurityStamp(context.Principal, userId);
      if (principal != null) {
        context.ReplacePrincipal(principal);
        context.ShouldRenew = true;
      } else {
        context.RejectPrincipal();
        await context.HttpContext.Authentication.LogOffAsync(Options);
      }
    }

    private async Task<ClaimsPrincipal> ValidateSecurityStamp(ClaimsPrincipal principal, string userId) {
      Guid userGuid;
      if (Guid.TryParse(userId, out userGuid)) {
        ClaimsPrincipal freshPrincipal = await Query.Execute(new ClaimsPrincipalByUserId(userGuid));
        var freshStamp = freshPrincipal.FindFirstValue(Options.ClaimsIdentity.SecurityStampClaimType);
        var currentStamp = principal.FindFirstValue(Options.ClaimsIdentity.SecurityStampClaimType);
        if (freshStamp == currentStamp) {
          return freshPrincipal;
        }
      }
      return null;
    }
  }
}
