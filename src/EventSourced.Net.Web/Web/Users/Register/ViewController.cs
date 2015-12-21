using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{
  public class ViewController : Controller
  {
    [HttpGet, Route("register")]
    public IActionResult Get() {
      return View("~/Web/Users/Register/Register.cshtml");
    }
  }
}
