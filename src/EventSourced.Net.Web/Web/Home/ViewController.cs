using System.IO;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace EventSourced.Net.Web.Home
{
  public class ViewController : Controller
  {
    private IApplicationEnvironment ApplicationEnvironment { get; }

    public ViewController(IApplicationEnvironment applicationEnvironment) {
      ApplicationEnvironment = applicationEnvironment;
    }

    [HttpGet, Route("home")]
    public IActionResult Home() {
      return View("~/Web/Home/Home.cshtml");
    }

    [HttpGet, Route("", Name = "HomeRoute"), Route("about"), Route("contact"), Route("login"), Route("register/{correlationId}", Name = "RegisterVerifyRoute"), Route("register/{correlationId}/redeem", Name = "RegisterRedeemRoute"), Route("register")]
    public IActionResult React() {
      string path = Path.GetFullPath(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "wwwroot/app.html"));
      var contents = System.IO.File.ReadAllText(path);
      return Content(contents, "text/html");
    }

    [HttpGet, Route("about1")]
    public IActionResult About() {
      return View("~/Web/Home/About.cshtml");
    }

    [HttpGet, Route("contact1")]
    public IActionResult Contact() {
      return View("~/Web/Home/Contact.cshtml");
    }
  }
}
