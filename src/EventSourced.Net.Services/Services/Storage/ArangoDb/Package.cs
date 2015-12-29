using ArangoDB.Client;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Storage.ArangoDb
{
  public class Package : IPackage
  {
    private Settings Settings { get; }

    public Package(Settings settings = null) {
      Settings = settings ?? new Settings();
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton(() =>
        new DatabaseSharedSetting {
          Url = Settings.ServerUrl,
          Database = Settings.DbName,
          WaitForSync = true,
        }
      );
      container.Register<IArangoDatabase>(() =>
        new ArangoDatabase(container.GetInstance<DatabaseSharedSetting>()), Lifestyle.Scoped);
    }
  }
}
