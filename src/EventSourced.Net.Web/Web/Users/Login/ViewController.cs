using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Login
{
  public class ViewController : Controller
  {
    private IProcessQuery Query { get; }

    public ViewController(IProcessQuery query) {
      Query = query;
    }

    [HttpGet, Route("login", Name = "LoginRoute")]
    public IActionResult Login() {
      return View("~/Web/Users/Login/Login.cshtml");
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(string login, string password) {
      Guid? data = await Query.Execute(new UserIdByLogin(login));
      if (data == null) {
        ModelState.AddModelError(nameof(login), "Invalid username or password.");
      }
      return View("~/Web/Users/Login/Login.cshtml");
    }
  }
}
