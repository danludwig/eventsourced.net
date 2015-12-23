using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace EventSourced.Net.Services.Web.Sockets
{
  internal sealed class WebSocketSharpServer : IServeWebSockets
  {
    public WebSocketServer Server { get; }
    private WebSocket Heartbeat { get; }
    private bool _isDisposing;
    private const string HeartbeatEndpoint = "/heartbeat";
    private const int Port = 3000;

    public WebSocketSharpServer() {
      Server = new WebSocketServer(Port);
      Server.Start();
      Server.AddWebSocketService<Heartbeat>(HeartbeatEndpoint);
      Heartbeat = new WebSocket($"ws://localhost:{Port}{HeartbeatEndpoint}");
      Heartbeat.Connect();

      Task.Factory.StartNew(() => {
        while (!_isDisposing) {
          Heartbeat.Send("Heartbeat OK");
          Task.Delay(5000).Wait();
        }
      });
    }

    public void Dispose() {
      _isDisposing = true;
      (Heartbeat as IDisposable)?.Dispose();
      Server?.Stop(CloseStatusCode.Normal, "Web socket service has been disposed.");
    }
  }
}