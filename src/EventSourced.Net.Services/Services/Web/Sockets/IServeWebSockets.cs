using System;
using WebSocketSharp.Server;

namespace EventSourced.Net.Services.Web.Sockets
{
  public interface IServeWebSockets : IDisposable
  {
    WebSocketServer Server { get; }
  }
}
