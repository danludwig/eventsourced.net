using System;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{

  public class ViewController : Controller
  {
    [HttpGet, Route("register")]
    public IActionResult Register() {
      return View("~/Web/Users/Register/Register.cshtml");
    }

    [HttpGet, Route("register/{correlationId}", Name = "RegisterVerifyRoute")]
    public IActionResult Verify(Guid correlationId) {
      var model = new VerifyViewModel {
        CorrelationId = correlationId,
      };
      return View("~/Web/Users/Register/Verify.cshtml", model);
    }
  }
}
