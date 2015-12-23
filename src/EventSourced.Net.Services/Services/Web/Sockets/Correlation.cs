using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace EventSourced.Net.Services.Web.Sockets
{
  public class Correlation : WebSocketBehavior
  {
    private readonly Guid _correlationId;

    public Correlation(Guid correlationId) {
      _correlationId = correlationId;
    }

    protected override void OnMessage(MessageEventArgs e) {
      Sessions.Broadcast(e.Data);
    }
  }
}
