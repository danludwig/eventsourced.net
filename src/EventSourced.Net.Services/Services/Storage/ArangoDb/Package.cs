using ArangoDB.Client;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Storage.ArangoDb
{
  public class Package : IPackage
  {
    public void RegisterServices(Container container) {
      container.RegisterSingleton<DatabaseSharedSetting>(() => new DatabaseSharedSetting {
        Url = "http://localhost:8529",
        Database = "EventSourced",
        CreateCollectionOnTheFly = true,
        WaitForSync = true,
      });
      container.Register<IArangoDatabase>(() =>
        new ArangoDatabase(container.GetInstance<DatabaseSharedSetting>()), Lifestyle.Scoped);
    }
  }
}
