using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Controllers;
using SimpleInjector;

namespace EventSourced.Net.Services.Web.Mvc
{
  public sealed class SimpleInjectorControllerActivator : IControllerActivator
  {
    private Container Container { get; }

    public SimpleInjectorControllerActivator(Container container) {
      Container = container;
    }

    public object Create(ActionContext context, Type controllerType) {
      return Container.GetInstance(controllerType);
    }
  }
}
