using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{
  public class ViewController : Controller
  {
    private IExecuteQuery Query { get; }

    public ViewController(IExecuteQuery query) {
      Query = query;
    }

    [HttpGet, Route("register")]
    public IActionResult Register() {
      string viewName = !User.IsSignedIn() ? "Register" : "AlreadyLoggedIn";
      return View($"~/Web/Users/Register/{viewName}.cshtml");
    }

    [HttpGet, Route("register/{correlationId}", Name = "RegisterVerifyRoute")]
    public IActionResult Verify(string correlationId) {
      if (User.IsSignedIn())
        return View("~/Web/Users/Register/AlreadyLoggedIn.cshtml");

      var model = new VerifyViewModel {
        CorrelationId = correlationId,
      };
      return View("~/Web/Users/Register/Verify.cshtml", model);
    }

    [HttpGet, Route("register/{correlationId}/redeem", Name = "RegisterRedeemRoute")]
    public async Task<IActionResult> Redeem(string correlationId, string token) {
      if (User.IsSignedIn())
        return View("~/Web/Users/Register/AlreadyLoggedIn.cshtml");

      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
      UserRegistrationRedeemData data = await Query
        .Execute(new UserRegistrationRedeemView(correlationGuid, token));
      if (data == null) return HttpNotFound();

      var model = new RedeemViewModel {
        CorrelationId = correlationId,
        Token = token,
        ContactValue = data.ContactValue,
        Purpose = data.Purpose,
      };
      return View("~/Web/Users/Register/Redeem.cshtml", model);
    }
  }
}
