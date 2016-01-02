using System;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Web.Mvc
{
  public class IdentityOptionsPackage : IPackage
  {
    private IServiceProvider ApplicationServices { get; }

    public IdentityOptionsPackage(IServiceProvider applicationServices) {
      ApplicationServices = applicationServices;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton<Func<IdentityOptions>>(() => {
        return () => ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
      });
      container.RegisterSingleton(() => container.GetInstance<Func<IdentityOptions>>()());
    }
  }
}
