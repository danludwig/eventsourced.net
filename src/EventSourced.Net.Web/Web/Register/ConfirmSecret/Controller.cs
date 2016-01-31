using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Register.ConfirmSecret
{
  [Return400BadRequstIfCommandRejected]
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }
    private IServeWebSockets WebSockets { get; }

    public Controller(ISendCommand command, IExecuteQuery query, IServeWebSockets webSockets) {
      Command = command;
      Query = query;
      WebSockets = webSockets;
    }

    [HttpGet, Route("register/{correlationId}", Name = "RegisterVerifyRoute")]
    public IActionResult GetView() {
      return new ServerRenderedAppViewResult(this, "Verify");
    }

    [HttpPost, Route("api/register/{correlationId}")]
    public async Task<IActionResult> PostApi(string correlationId, [FromBody] PostApiRequest model) {
      if (model == null) return HttpBadRequest(new object());
      //await Task.Delay(1000);
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
      UserContactChallengeTokenData data = await Query.Execute(new UserContactChallengeTokenView(correlationGuid));
      if (data == null) return HttpNotFound();

      await Command.SendAsync(new VerifyUserContactChallengeResponse(data.UserId, correlationGuid, model.Code))
        .ConfigureAwait(false);

      var location = Url.RouteUrl("RegisterRedeemRoute", new { token = data.Token, });
      return new CreatedResult(location, new PostApiResponse(data));
    }
  }
}
