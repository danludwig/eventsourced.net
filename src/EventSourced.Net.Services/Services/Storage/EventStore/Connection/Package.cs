using CommonDomain.Persistence;
using EventSourced.Net.Services.Storage.EventStore.Repository;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  public class Package : IPackage
  {
    private readonly Configuration.Connection _connection;

    public Package(Configuration.Connection connection) {
      connection = connection ?? new Configuration.Connection();
      _connection = connection;
    }

    public void RegisterServices(Container container) {
      container.RegisterSingleton<IConfigureConnection>(() => _connection);
      container.RegisterSingleton<IConstructConnection, Factory>();
      container.RegisterSingleton<IProvideConnection, Provider>();
      container.Register<IRepository, AggregateRepository>(Lifestyle.Transient);
    }
  }
}
