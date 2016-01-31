using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Register.ChallengeContact
{
  [Return400BadRequstIfCommandRejected]
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }

    public Controller(ISendCommand command, IExecuteQuery query) {
      Command = command;
      Query = query;
    }

    [HttpGet, Route("register")]
    public IActionResult GetView() {
      return new ServerRenderedAppViewResult(this, "Register");
    }

    [HttpPost, Route("api/register")]
    public async Task<IActionResult> PostApi([FromBody] PostApiRequest model) {
      if (model == null) return HttpBadRequest(new object());
      //await Task.Delay(1000);
      ShortGuid correlationId = Guid.NewGuid();
      Guid? userIdByLogin = await Query.Execute(new UserIdByLogin(model.EmailOrPhone));

      await Command.SendAsync(new PrepareUserRegistrationChallenge(correlationId, model.EmailOrPhone,
        userIdByLogin, User)).ConfigureAwait(false);

      string location = Url.RouteUrl("RegisterVerifyRoute", new { correlationId });
      return new CreatedResult(location, new { correlationId, });
    }
  }
}
