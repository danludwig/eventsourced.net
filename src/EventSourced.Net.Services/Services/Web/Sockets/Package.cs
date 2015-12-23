using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Web.Sockets
{
  public class Package : IPackage
  {
    private ServerConfiguration Server { get; }

    public Package(ServerConfiguration server = null) {
      Server = server;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton(Server ?? ServerSettings.Default);
      container.RegisterSingleton<IServeWebSockets, WebSocketSharpServer>();
    }
  }
}
