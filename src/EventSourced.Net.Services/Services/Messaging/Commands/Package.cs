﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Messaging.Commands
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
      container.RegisterSingleton<ISendCommand, ImmediatelyConsistentCommandSender>();
      container.Register(typeof(IHandleCommand<>), HandlerAssemblies, Lifestyle.Transient);
    }
  }
}
