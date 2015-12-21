using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Messaging.Commands
{
  public class Package : IPackage
  {
    private readonly IEnumerable<Assembly> _handlerAssemblies;

    public Package(params Assembly[] handlerAssemblies) {
      if (handlerAssemblies == null || !handlerAssemblies.Any()) {
        handlerAssemblies = new[] { typeof(IHandleCommand<>).Assembly };
      }
      _handlerAssemblies = handlerAssemblies;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton<ISendCommand, ImmediatelyConsistentCommandSender>();
      container.Register(typeof(IHandleCommand<>), _handlerAssemblies, Lifestyle.Transient);
    }
  }
}
