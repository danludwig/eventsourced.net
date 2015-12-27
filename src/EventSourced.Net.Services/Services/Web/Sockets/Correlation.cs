using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace EventSourced.Net.Services.Web.Sockets
{
  public class Correlation : WebSocketBehavior
  {
    private Guid CorrelationId { get; }

    public Correlation(Guid correlationId) {
      CorrelationId = correlationId;
    }

    protected override void OnMessage(MessageEventArgs e) {
      Sessions.Broadcast(e.Data);
    }
  }
}
