using System.Threading.Tasks;
using EventSourced.Net.Services.Web.Mvc;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Login
{
  [Return400BadRequstIfCommandRejected]
  public class ApiController : Controller
  {
    private ISendCommand Command { get; }

    public ApiController(ISendCommand command) {
      Command = command;
    }

    [HttpPost, Route("api/login")]
    public async Task<IActionResult> PostLogin(string login, string password, string returnUrl = null) {
      await Command.SendAsync(new LogUserIn(login, password, HttpContext.Authentication));
      Response.Headers["Location"] = returnUrl ?? Url.RouteUrl("HomeRoute");
      return Ok();
    }
  }
}
