using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Register.CreateLogin
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

    [HttpGet, Route("register/{correlationId}/redeem", Name = "RegisterRedeemRoute")]
    public async Task<IActionResult> GetView(string correlationId, string token) {
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();

      UserContactChallengeRedeemData data = await Query
        .Execute(new UserContactChallengeRedeemView(correlationGuid, token));
      if (data == null) return HttpNotFound();
      var model = new ReduxServerRenderState(this, new ReduxAppState(User) {
        Register = new Register.ReduxState(data),
      });
      return new ServerRenderedAppViewResult(this, "Create login", model);
    }

    [HttpPost, Route("api/register/{correlationId}/redeem")]
    public async Task<IActionResult> PostApi(string correlationId, string token, [FromBody] PostApiRequest model) {
      if (model == null) return HttpBadRequest(new object());
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
      //await Task.Delay(1000);

      UserContactChallengeRedeemData data = await Query
        .Execute(new UserContactChallengeRedeemView(correlationGuid, token));
      if (data == null) return HttpNotFound();
      Guid? userIdByUsername = await Query.Execute(new UserIdByLogin(model.Username));
      Guid? userIdByContact = await Query.Execute(new UserIdByLogin(data.ContactValue));

      await Command.SendAsync(new RedeemUserRegistrationChallenge(data.UserId, correlationGuid, token,
        data.ContactValue, userIdByContact, model.Username, userIdByUsername, model.Password, model.PasswordConfirmation))
          .ConfigureAwait(false);

      WebSockets.AddCorrelationService(correlationId);
      var correlationUrl = WebSockets.GetCorrelationUri(correlationId).ToString();
      Response.Headers.Add("X-Correlation-Socket", correlationUrl);
      var location = Url.RouteUrl("LoginRoute");
      return new CreatedResult(location, new { CorrelationId = correlationId, });
    }
  }
}
