using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Services.Storage.EventStore.Subscriptions
{
  public class Package : IPackage
  {
    // todo: only starting up subscriptions from web client for simplicity, should be done in console app (separate process)
    public void RegisterServices(Container container) {
      container.RegisterSingleton<ResolvedEventPublisher>();
      container.Register<SubscriptionClient[]>(() => {
        // todo: json configuration for subscription clients
        string streamName = "$ce-user";
        string groupName = "user-publisher";
        PersistentSubscriptionSettingsBuilder settings = PersistentSubscriptionSettings.Create()
          .ResolveLinkTos()
          .StartFrom(0)
          .MinimumCheckPointCountOf(1)
          .MaximumCheckPointCountOf(1)
        ;
        try {
          IEventStoreConnection connection = container.GetInstance<Connection.IProvideConnection>().GetConnectionAsync().Result;
          // todo: should not be creating the persistent subscription from code http://docs.geteventstore.com/dotnet-api/3.3.1/competing-consumers/
          connection.CreatePersistentSubscriptionAsync(streamName, groupName, settings, new UserCredentials("admin", "changeit")).Wait();
        } catch (AggregateException ex) when (ex.InnerException.Message.Equals($"Subscription group {groupName} on stream {streamName} already exists")) {
          // subscription already exists
        }
        var userClient = new SubscriptionClient(streamName, groupName,
          container.GetInstance<Connection.IProvideConnection>(), container.GetInstance<ResolvedEventPublisher>());
        return new[] { userClient };
      }, Lifestyle.Singleton);
    }
  }
}
