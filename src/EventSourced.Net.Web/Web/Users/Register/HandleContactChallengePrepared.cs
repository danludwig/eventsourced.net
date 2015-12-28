using System.Threading.Tasks;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.Services.Web.Sockets;
using WebSocketSharp;

namespace EventSourced.Net.Web.Users.Register
{
  public class HandleContactChallengePrepared :
    IHandleEvent<ContactEmailChallengePrepared>,
    IHandleEvent<ContactSmsChallengePrepared>
  {
    private IServeWebSockets WebSockets { get; }

    public HandleContactChallengePrepared(IServeWebSockets webSockets) {
      WebSockets = webSockets;
    }

    public Task HandleAsync(ContactEmailChallengePrepared message) {
      return HandleAsync((ContactChallengePrepared)message);
    }

    public Task HandleAsync(ContactSmsChallengePrepared message) {
      return HandleAsync((ContactChallengePrepared)message);
    }

    private Task HandleAsync(ContactChallengePrepared message) {
      if (!WebSockets.IsCorrelationService(message.CorrelationId)) return Task.FromResult(true);

      using (WebSocket client = WebSockets.CreateCorrelationClient(message.CorrelationId)) {
        client.Send(new {
          Type = message.GetType().Name,
          Purpose = message.Purpose.ToString(),
        });
      }
      WebSockets.RemoveCorrelationService(message.CorrelationId);
      return Task.FromResult(true);
    }
  }
}
