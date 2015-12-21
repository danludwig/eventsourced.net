using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Messaging.Events
{
  public class ImmediateConsistencyPackage : IPackage
  {
    private readonly IEnumerable<Assembly> _handlerAssemblies;

    public ImmediateConsistencyPackage(params Assembly[] handlerAssemblies) {
      if (handlerAssemblies == null || !handlerAssemblies.Any()) {
        handlerAssemblies = new[] { typeof(IHandleCommand<>).Assembly };
      }
      _handlerAssemblies = handlerAssemblies;
    }

    public void RegisterServices(Container container) {
      container.Register(typeof(IPublishEvent<>), typeof(ImmediatelyConsistentEventPublisher<>), Lifestyle.Singleton);
      container.RegisterCollection(typeof(IHandleEvent<>), _handlerAssemblies);
    }
  }
}
