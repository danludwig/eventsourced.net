using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Messaging.Events
{
  public class Package : IPackage
  {
    private IEnumerable<Assembly> HandlerAssemblies { get; }

    public Package(params Assembly[] handlerAssemblies) {
      if (handlerAssemblies == null || !handlerAssemblies.Any()) {
        handlerAssemblies = new[] { typeof(IHandleCommand<>).Assembly };
      }
      HandlerAssemblies = handlerAssemblies;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton(typeof(IPublishEvent), typeof(ImmediatelyConsistentEventPublisher));
      container.RegisterCollection(typeof(IHandleEvent<>), HandlerAssemblies);
    }
  }
}
