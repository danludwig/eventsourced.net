using System;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{
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
      Guid correlationId = Guid.NewGuid();
      WebSockets.AddCorrelationService(correlationId);

      await Command.SendAsync(new PrepareUserContactChallenge(correlationId, emailOrPhone))
        .ConfigureAwait(false);

      Response.Headers.Add("X-Correlation-Socket", WebSockets.GetCorrelationUri(correlationId).ToString());
      return new CreatedResult(Url.RouteUrl("RegisterVerifyRoute", new { correlationId }),
        new { CorrelationId = correlationId, });
    }

    [HttpPost, Route("api/register/{correlationId}")]
    public async Task<IActionResult> PostVerify(Guid correlationId, string code) {
      UserContactChallengeTokenView view = await Query.Execute(new UserContactChallengeTokenQuery(correlationId));
      if (view == null) return HttpNotFound(); // better yet, http bad request (400)

      await Command.SendAsync(new VerifyUserContactChallengeResponse(view.UserId, correlationId, code))
        .ConfigureAwait(false);

      return new CreatedResult(Url.RouteUrl("RegisterRedeemRoute", new { token = view.Token, }),
        new { CorrelationId = correlationId, });
    }

    [HttpPost, Route("api/register/{correlationId}/redeem")]
    public async Task<IActionResult> PostRedeem(Guid correlationId, string token, string password, string passwordConfirmation) {
      Guid? userId = await Query.Execute(new UserIdByContactChallengeCorrelationId(correlationId));
      if (userId == null) return HttpNotFound(); // better yet, http bad request (400)

      await Command.SendAsync(new CreateUserPassword(userId.Value, correlationId, token, password, passwordConfirmation))
        .ConfigureAwait(false);

      return new CreatedResult(Url.RouteUrl("LoginRoute"),
        new { CorrelationId = correlationId, });
    }
  }
}