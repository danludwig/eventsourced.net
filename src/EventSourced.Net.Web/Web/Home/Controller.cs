using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Home
{
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    [HttpGet, Route("", Name = "HomeRoute")]
    public IActionResult Home() {
      return this.ServerRenderedView("Home");
    }

    [HttpGet, Route("about")]
    public IActionResult About() {
      return this.ServerRenderedView("About");
    }

    [HttpGet, Route("contact")]
    public IActionResult Contact() {
      return this.ServerRenderedView("Contact");
    }
  }
}
