using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Home
{
  public class ApiController : Controller
  {
    [HttpGet, Route("api"), Route("api/about"), Route("api/contact"), Route("api/login"), Route("api/register/{correlationId}"), Route("api/register")]
    public IActionResult GetInitialState() {
      //System.Threading.Thread.Sleep(3000);
      return Ok(new ReduxStateResponse(User));
    }
  }
}
