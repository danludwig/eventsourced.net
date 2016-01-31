using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Register.ValidateUsername
{
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    private IExecuteQuery Query { get; }

    public Controller(IExecuteQuery query) {
      Query = query;
    }

    [HttpPost, Route("api/validate-username")]
    public async Task<IActionResult> PostApi([FromBody] PostApiRequest model) {
      //await Task.Delay(1000);
      if (model == null) return HttpBadRequest(new object());
      Guid? userIdByLogin = await Query.Execute(new UserIdByLogin(model.Username));
      Net.ValidateUsername validation = new Net.ValidateUsername(model.Username, userIdByLogin);
      CommandRejectionReason? reasonInvalid = validation.Errors
        .SelectMany(x => x.Value.Select(y => y.Reason))
        .Cast<CommandRejectionReason?>().FirstOrDefault();
      var payload = new PostApiResponse { ReasonInvalid = reasonInvalid, };
      return payload.IsAvailable ? Ok(payload) : HttpBadRequest(payload) as IActionResult;
    }
  }
}
