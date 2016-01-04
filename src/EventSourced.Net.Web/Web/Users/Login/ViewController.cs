using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Login
{
  public class ViewController : Controller
  {
    private ISendCommand Command { get; }

    public ViewController(ISendCommand command) {
      Command = command;
    }

    [HttpGet, Route("login", Name = "LoginRoute")]
    public IActionResult Login() {
      return View("~/Web/Users/Login/Login.cshtml", new LoginViewModel());
    }

    [HttpPost, Route("logoff")]
    public async Task<IActionResult> Logoff() {
      await Command.SendAsync(new LogUserOff(HttpContext.Authentication));
      return Redirect("~/");
    }
  }
}
