using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Home
{
  public class ViewController : Controller
  {
    [HttpGet, Route("")]
    public IActionResult Home() {
      return View("~/Web/Home/Home.cshtml");
    }

    [HttpGet, Route("about")]
    public IActionResult About() {
      return View("~/Web/Home/About.cshtml");
    }

    [HttpGet, Route("contact")]
    public IActionResult Contact() {
      return View("~/Web/Home/Contact.cshtml");
    }

    [HttpGet, Route("errors/404")]
    public IActionResult Error404() {
      return View("~/Web/Home/404.cshtml");
    }

    [HttpGet, Route("errors/400")]
    public IActionResult Error400() {
      return View("~/Web/Home/400.cshtml");
    }
  }
}
