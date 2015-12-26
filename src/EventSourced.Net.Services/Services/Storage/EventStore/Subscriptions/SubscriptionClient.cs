using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace EventSourced.Net.Services.Storage.EventStore.Subscriptions
{
  public class SubscriptionClient : IDisposable
  {
    private string StreamName { get; }
    private string GroupName { get; }
    private Connection.IProvideConnection ConnectionProvider { get; }
    private EventStorePersistentSubscriptionBase Subscription { get; set; }

    public SubscriptionClient(string streamName, string groupName, Connection.IProvideConnection connectionProvider) {
      StreamName = streamName;
      GroupName = groupName;
      ConnectionProvider = connectionProvider;
      ConnectSubscription().Wait();
    }

    private async Task ConnectSubscription() {
      IEventStoreConnection connection = await ConnectionProvider.GetConnectionAsync();
      Subscription = connection.ConnectToPersistentSubscription(StreamName, GroupName,
        EventAppeared, SubscriptionDropped, userCredentials: new UserCredentials("admin", "changeit"), bufferSize: 2, autoAck: false);
    }

    private async void SubscriptionDropped(EventStorePersistentSubscriptionBase subscription, SubscriptionDropReason reason, Exception ex) {
      await ConnectSubscription();
    }

    private void EventAppeared(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent) {
      subscription.Acknowledge(resolvedEvent);
    }

    public void Dispose() {
      Subscription.Stop(TimeSpan.FromSeconds(15));
    }
  }
}
