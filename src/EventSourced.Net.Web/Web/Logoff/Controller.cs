using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Logoff
{
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    private ISendCommand Command { get; }

    public Controller(ISendCommand command) {
      Command = command;
    }

    [HttpPost, Route("api/logoff")]
    public async Task<IActionResult> PostApi(string returnUrl) {
      await Command.SendAsync(new LogUserOff(HttpContext.Authentication));
      Response.Headers["Location"] = returnUrl ?? Url.RouteUrl("HomeRoute");
      return Ok(new PostApiResponse());
    }
  }
}
