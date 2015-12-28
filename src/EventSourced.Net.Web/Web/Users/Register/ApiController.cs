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
    private IProcessQuery Query { get; }
    private IServeWebSockets WebSockets { get; }

    public ApiController(ISendCommand command, IProcessQuery query, IServeWebSockets webSockets) {
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
      UserContactChallengeView data = await Query.Execute(new UserContactChallengeByCorrelationId(correlationId));
      if (data == null) return HttpNotFound(); // better yet, http bad request (400)

      await Command.SendAsync(new VerifyUserContactChallengeResponse(data.UserId, correlationId, code))
        .ConfigureAwait(false);

      return new CreatedResult(Url.RouteUrl("RegisterVerifyRoute", new { correlationId }),
        new { CorrelationId = correlationId, });
    }
  }
}