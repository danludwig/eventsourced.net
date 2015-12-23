using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Web.Sockets
{
    public class Package : IPackage
    {
      public void RegisterServices(Container container)
      {
        container.RegisterSingleton<IServeWebSockets, WebSocketSharpServer>();
      }
    }
}
