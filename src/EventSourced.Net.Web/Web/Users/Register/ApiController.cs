using System;
using System.Threading.Tasks;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.AspNet.Mvc;
using WebSocketSharp;

namespace EventSourced.Net.Web.Users.Register
{
  public class ApiController : Controller
  {
    private IServeWebSockets WebSockets { get; }

    public ApiController(IServeWebSockets webSockets) {
      WebSockets = webSockets;
    }

    [HttpPost, Route("api/register")]
    public IActionResult Post(string emailOrPhone) {
      Guid correlationId = Guid.NewGuid();
      WebSockets.AddCorrelationService(correlationId);

      // todo: remove this code that pretends to send a correlated event to the web
      Task.Factory.StartNew(async () => {
        using (WebSocket client = WebSockets.CreateCorrelationClient(correlationId)) {
          await Task.Delay(1).ConfigureAwait(false);
          client.Send("pretend this is a serialized event data object");
        }
      });

      return new CreatedResult(WebSockets.GetCorrelationUri(correlationId), new {
        CorrelationId = correlationId,
      });
    }
  }
}