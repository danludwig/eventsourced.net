using System;
using System.Threading.Tasks;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{
  public class ApiController : Controller
  {
    private ISendCommand Command { get; }
    private IServeWebSockets WebSockets { get; }

    public ApiController(ISendCommand command, IServeWebSockets webSockets) {
      WebSockets = webSockets;
      Command = command;
    }

    [HttpPost, Route("api/register")]
    public async Task<IActionResult> PostChallenge(string emailOrPhone) {
      Guid correlationId = Guid.NewGuid();
      WebSockets.AddCorrelationService(correlationId);

      await Command.SendAsync(new PrepareUserContactChallenge(correlationId, emailOrPhone))
        .ConfigureAwait(false);

      Response.Headers.Add("X-Correlation-Socket", WebSockets.GetCorrelationUri(correlationId).ToString());
      return new CreatedResult(Url.RouteUrl("RegisterVerifyRoute", new { correlationId }),
        new {
          CorrelationId = correlationId,
        });
    }
  }
}