using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{
  [Return400BadRequstIfCommandRejected]
  public class ApiController : Controller
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }
    private IServeWebSockets WebSockets { get; }

    public ApiController(ISendCommand command, IExecuteQuery query, IServeWebSockets webSockets) {
      Command = command;
      Query = query;
      WebSockets = webSockets;
    }

    [HttpPost, Route("api/register")]
    public async Task<IActionResult> PostChallenge(string emailOrPhone) {
      ShortGuid correlationId = Guid.NewGuid();

      await Command.SendAsync(new PrepareUserContactChallenge(correlationId, emailOrPhone))
        .ConfigureAwait(false);

      string location = Url.RouteUrl("RegisterVerifyRoute", new { correlationId });
      return new CreatedResult(location, new { CorrelationId = correlationId, });
    }

    [HttpPost, Route("api/register/{correlationId}")]
    public async Task<IActionResult> PostVerify(string correlationId, string code) {
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
      UserContactChallengeTokenView view = await Query.Execute(new UserContactChallengeTokenQuery(correlationGuid));
      if (view == null) return HttpNotFound(); // better yet, http bad request (400)

      await Command.SendAsync(new VerifyUserContactChallengeResponse(view.UserId, correlationGuid, code))
        .ConfigureAwait(false);

      var location = Url.RouteUrl("RegisterRedeemRoute", new { token = view.Token, });
      return new CreatedResult(location, new { CorrelationId = correlationId, });
    }

    [HttpPost, Route("api/register/{correlationId}/redeem")]
    public async Task<IActionResult> PostRedeem(string correlationId, string token, string password, string passwordConfirmation) {
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
      Guid? userId = await Query.Execute(new UserIdByContactChallengeCorrelationId(correlationGuid));
      if (userId == null) return HttpNotFound(); // better yet, http bad request (400)

      await Command.SendAsync(new RedeemUserContactChallenge(userId.Value, correlationGuid, token, password, passwordConfirmation))
        .ConfigureAwait(false);

      WebSockets.AddCorrelationService(correlationId);
      var correlationUrl = WebSockets.GetCorrelationUri(correlationId).ToString();
      Response.Headers.Add("X-Correlation-Socket", correlationUrl);
      var location = Url.RouteUrl("LoginRoute");
      return new CreatedResult(location, new { CorrelationId = correlationId, });
    }
  }
}