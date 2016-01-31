using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Errors
{
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    [HttpGet, Route("errors/404")]
    public IActionResult Error404() {
      return new ServerRenderedAppViewResult(this, "404 Not Found");
    }

    [HttpGet, Route("errors/400")]
    public IActionResult Error400() {
      return new ServerRenderedAppViewResult(this, "400 Bad Request");
    }
  }
}
