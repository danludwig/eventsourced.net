using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Login
{
  public class ViewController : Controller
  {
    private IExecuteQuery Query { get; }
    private ISendCommand Command { get; }

    public ViewController(IExecuteQuery query, ISendCommand command) {
      Query = query;
      Command = command;
    }

    [HttpGet, Route("login", Name = "LoginRoute")]
    public IActionResult Login() {
      return View("~/Web/Users/Login/Login.cshtml", new LoginViewModel());
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(string login, string password) {
      Guid? userId = await Query.Execute(new UserIdByLogin(login));
      if (!userId.HasValue) {
        return LoginErrorView(login);
      }
      try {
        await Command.SendAsync(new VerifyUserLogin(userId.Value, login, password));
      } catch {
        return LoginErrorView(login);
      }

      await Command.SendAsync(new LogUserIn(userId.Value, login, HttpContext.Authentication));
      return Redirect("~/");
    }

    private ViewResult LoginErrorView(string login) {
      LoginViewModel model = new LoginViewModel { Login = login };
      const string view = "~/Web/Users/Login/Login.cshtml";
      ModelState.AddModelError("", "Invalid username or password.");
      return View(view, model);
    }

    [HttpPost, Route("logoff")]
    public async Task<IActionResult> Logoff() {
      await Command.SendAsync(new LogUserOff(HttpContext.Authentication));
      return Redirect("~/");
    }
  }
}
