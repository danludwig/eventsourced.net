using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Home
{
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    [HttpGet, Route("", Name = "HomeRoute")]
    public IActionResult Home() {
      return new ServerRenderedAppViewResult(this, "Home");
    }

    [HttpGet, Route("about")]
    public IActionResult About() {
      return new ServerRenderedAppViewResult(this, "About");
    }

    [HttpGet, Route("contact")]
    public IActionResult Contact() {
      return new ServerRenderedAppViewResult(this, "Contact");
    }
  }
}
