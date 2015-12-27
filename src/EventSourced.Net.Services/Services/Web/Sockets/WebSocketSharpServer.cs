using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace EventSourced.Net.Services.Web.Sockets
{
  [UsedImplicitly]
  internal sealed class WebSocketSharpServer : IServeWebSockets
  {
    private ServerSettings Settings { get; }
    public WebSocketServer Server { get; }
    public Uri BaseUri { get; }

    private WebSocket Heartbeat { get; }
    private const string HeartbeatEndpoint = "heartbeat";
    private bool _isDisposing;

    public WebSocketSharpServer(ServerSettings settings) {
      Settings = settings;
      Server = new WebSocketServer(Settings.Port);
      BaseUri = new Uri($"ws://localhost:{Server.Port}");
      Server.Start();
      Server.AddWebSocketService<Heartbeat>($"/{HeartbeatEndpoint}");
      Heartbeat = new WebSocket($"{BaseUri}{HeartbeatEndpoint}");
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
