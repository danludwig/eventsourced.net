using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Web.Mvc
{
  public class ApplicationEnvironmentPackage : IPackage
  {
    private IServiceProvider ApplicationServices { get; }

    public ApplicationEnvironmentPackage(IServiceProvider applicationServices) {
      ApplicationServices = applicationServices;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton(() => ApplicationServices.GetService<IApplicationEnvironment>());
    }
  }
}
