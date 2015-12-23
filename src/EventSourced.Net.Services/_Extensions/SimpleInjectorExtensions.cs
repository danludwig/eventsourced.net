using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net
{
  public static class SimpleInjectorExtensions
  {
    public static void RegisterPackages(this Container container, params IPackage[] packages) {
      foreach (IPackage package in packages) {
        package.RegisterServices(container);
      }
    }
  }
}
