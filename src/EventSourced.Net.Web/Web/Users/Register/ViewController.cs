using System;
using System.Threading.Tasks;
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
      return View("~/Web/Users/Register/Register.cshtml");
    }

    [HttpGet, Route("register/{correlationId}", Name = "RegisterVerifyRoute")]
    public IActionResult Verify(Guid correlationId) {
      var model = new VerifyViewModel {
        CorrelationId = correlationId,
      };
      return View("~/Web/Users/Register/Verify.cshtml", model);
    }

    [HttpGet, Route("register/{correlationId}/redeem", Name = "RegisterRedeemRoute")]
    public async Task<IActionResult> Redeem(Guid correlationId, string token) {
      UserContactChallengeCreatePasswordView view = await Query
        .Execute(new UserContactChallengeCreatePasswordQuery(correlationId, token));
      if (view == null) return HttpNotFound();

      var model = new RedeemViewModel {
        CorrelationId = correlationId,
        Token = token,
        ContactValue = view.ContactValue,
      };
      return View("~/Web/Users/Register/Redeem.cshtml", model);
    }
  }
}
