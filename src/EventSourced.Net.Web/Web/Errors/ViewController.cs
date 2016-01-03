using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Errors
{
  public class ViewController : Controller
  {
    [HttpGet, Route("errors/404")]
    public IActionResult Error404() {
      return View("~/Web/Errors/404.cshtml");
    }

    [HttpGet, Route("errors/400")]
    public IActionResult Error400() {
      return View("~/Web/Errors/400.cshtml");
    }
  }
}
