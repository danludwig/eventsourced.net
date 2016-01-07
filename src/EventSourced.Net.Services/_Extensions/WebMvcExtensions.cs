using System.Threading.Tasks;
using EventSourced.Net.Services.Web.Mvc;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;

namespace EventSourced.Net
{
  public static class WebMvcExtensions
  {
    public static IApplicationBuilder UseExecutionContextScope(this IApplicationBuilder app) {
      return app.UseMiddleware<ExecutionContextScopeMiddleware>();
    }

    public static IServiceCollection AddExternalCookieAuthentication(this IServiceCollection services) {
      return services.AddAuthentication(options => {
        options.SignInScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
      });
    }

    public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app) {
      var options = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
      return app
        .UseCookieAuthentication(options.Cookies.ApplicationCookie)
        .UseCookieAuthentication(options.Cookies.ExternalCookie)
      ;
    }

    public static async Task LogOffAsync(this AuthenticationManager authentication, IdentityOptions options) {
      await authentication.SignOutAsync(options.Cookies.ApplicationCookieAuthenticationScheme);
      await authentication.SignOutAsync(options.Cookies.ExternalCookieAuthenticationScheme);
    }
  }
}
