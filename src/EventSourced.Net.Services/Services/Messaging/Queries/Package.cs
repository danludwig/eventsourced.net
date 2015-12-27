using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Messaging.Queries
{
  public class Package : IPackage
  {
    private IEnumerable<Assembly> HandlerAssemblies { get; }

    public Package(params Assembly[] handlerAssemblies) {
      if (handlerAssemblies == null || !handlerAssemblies.Any()) {
        handlerAssemblies = new[] { typeof(IHandleQuery<,>).Assembly };
      }
      HandlerAssemblies = handlerAssemblies;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton<IProcessQuery, QueryProcessor>();
      container.Register(typeof(IHandleQuery<,>), HandlerAssemblies, Lifestyle.Transient);
    }
  }
}
