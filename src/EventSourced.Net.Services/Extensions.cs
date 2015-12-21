using Microsoft.Extensions.Configuration;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net
{
  public static class Extensions
  {
    public static void RegisterPackages(this Container container, params IPackage[] packages) {
      foreach (IPackage package in packages) {
        package.RegisterServices(container);
      }
    }

    public static T GetConfiguration<T>(this IConfiguration configuration, string section) {
      return configuration.GetSection(section).Get<T>();
    }
  }
}
