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
      Guid? data = await Query.Execute(new UserIdByLogin(login));
      LoginViewModel model = new LoginViewModel { Login = login };
      if (data == null) {
        ModelState.AddModelError("", "Invalid username or password.");
      } else {
        try {
          await Command.SendAsync(new VerifyUserLogin(data.Value, login, password));
          return Redirect("~/");
        } catch {
          ModelState.AddModelError("", "Invalid username or password.");
        }
      }
      return View("~/Web/Users/Login/Login.cshtml", model);
    }
  }
}
