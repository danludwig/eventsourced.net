using System.Threading.Tasks;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Events;
using EventSourced.Net.Services.Web.Sockets;
using WebSocketSharp;

namespace EventSourced.Net.Web.Users.Register
{
  public class SendContactChallengeRedemptionToWebSocket :
    IHandleEvent<ContactChallengeRedemptionReversed>,
    IHandleEvent<ContactChallengeRedemptionConcluded>
  {
    private IServeWebSockets WebSockets { get; }

    public SendContactChallengeRedemptionToWebSocket(IServeWebSockets webSockets) {
      WebSockets = webSockets;
    }

    public Task HandleAsync(ContactChallengeRedemptionReversed message) {
      if (!WebSockets.IsCorrelationService(message.CorrelationId)) return Task.FromResult(true);

      using (WebSocket client = WebSockets.CreateCorrelationClient(message.CorrelationId)) {
        client.Send(new {
          Type = message.GetType().Name,
        });
      }
      WebSockets.RemoveCorrelationService(message.CorrelationId);
      return Task.FromResult(true);
    }

    public Task HandleAsync(ContactChallengeRedemptionConcluded message) {
      if (!WebSockets.IsCorrelationService(message.CorrelationId)) return Task.FromResult(true);

      using (WebSocket client = WebSockets.CreateCorrelationClient(message.CorrelationId)) {
        client.Send(new {
          Type = message.GetType().Name,
        });
      }
      WebSockets.RemoveCorrelationService(message.CorrelationId);
      return Task.FromResult(true);
    }
  }
}
