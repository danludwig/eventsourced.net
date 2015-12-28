using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{

  public class ViewController : Controller
  {
    private IProcessQuery Query { get; }

    public ViewController(IProcessQuery query) {
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
      UserContactChallengeView data = await Query.Execute(new UserContactChallengeByCorrelationId(correlationId));
      if (data == null) return HttpNotFound();

      var model = new RedeemViewModel {
        CorrelationId = correlationId,
        Token = token,
        ContactValue = data.ContactValue,
      };
      return View("~/Web/Users/Register/Redeem.cshtml", model);
    }
  }
}
