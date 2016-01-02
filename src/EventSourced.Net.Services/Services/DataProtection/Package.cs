using System;
using Microsoft.AspNet.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.DataProtection
{
  public class Package : IPackage
  {
    private IServiceProvider ApplicationServices { get; }

    public Package(IServiceProvider applicationServices) {
      ApplicationServices = applicationServices;
    }

    public void RegisterServices(Container container) {
      Domain.Users.ContactChallengers.DataProtectionTokenProvider
        .SetProviderFactory(ApplicationServices.GetRequiredService<IDataProtectionProvider>);
    }
  }
}
