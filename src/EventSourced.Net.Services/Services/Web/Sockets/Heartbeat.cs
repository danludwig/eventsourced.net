using WebSocketSharp;
using WebSocketSharp.Server;

namespace EventSourced.Net.Services.Web.Sockets
{
  public class Heartbeat : WebSocketBehavior
  {
    protected override void OnMessage(MessageEventArgs e) {
      Sessions.Broadcast(e.Data);
    }
  }
}
