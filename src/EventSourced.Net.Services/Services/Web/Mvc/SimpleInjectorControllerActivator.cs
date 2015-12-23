using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Controllers;
using SimpleInjector;

namespace EventSourced.Net.Services.Web.Mvc
{
  public sealed class SimpleInjectorControllerActivator : IControllerActivator
  {
    private readonly Container _container;

    public SimpleInjectorControllerActivator(Container container) {
      _container = container;
    }

    public object Create(ActionContext context, Type controllerType) {
      return _container.GetInstance(controllerType);
    }
  }
}
