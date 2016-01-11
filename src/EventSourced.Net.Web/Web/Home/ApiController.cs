using System.Security.Claims;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Home
{
  public class ApiController : Controller
  {
    [HttpGet, Route("api")]
    public IActionResult GetInitialState() {
      //System.Threading.Thread.Sleep(3000);
      string username = User.GetUserName();
      return Ok(new {
        Server = new {
          Initialized = true,
          Unavailable = false,
        },
        Data = new {
          User = new {
            Username = username,
          },
        },
      });
    }
  }
}
