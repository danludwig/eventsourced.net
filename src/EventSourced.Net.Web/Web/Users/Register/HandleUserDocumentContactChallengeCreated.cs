using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Sockets;
using WebSocketSharp;

namespace EventSourced.Net.Web.Users.Register
{
  public class HandleUserDocumentContactChallengeCreated : IHandleEvent<UserContactChallengeViewPrepared>
  {
    private IServeWebSockets WebSockets { get; }

    public HandleUserDocumentContactChallengeCreated(IServeWebSockets webSockets) {
      WebSockets = webSockets;
    }

    public Task HandleAsync(UserContactChallengeViewPrepared message) {
      if (!WebSockets.IsCorrelationService(message.ChallengeId)) return Task.FromResult(true);

      using (WebSocket client = WebSockets.CreateCorrelationClient(message.ChallengeId)) {
        client.Send(new {
          CorrelationId = message.ChallengeId,
          Type = message.GetType().Name,
          Purpose = message.Purpose.ToString(),
        });
      }
      WebSockets.RemoveCorrelationService(message.ChallengeId);
      return Task.FromResult(true);
    }
  }
}
