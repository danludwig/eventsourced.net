using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using JetBrains.Annotations;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EventSourced.Net.Services.Storage.EventStore.Subscriptions
{
  [UsedImplicitly]
  public class ResolvedEventPublisher
  {
    private Container Container { get; }

    public ResolvedEventPublisher(Container container) {
      Container = container;
    }

    public async Task PublishAsync(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent) {
      try {
        object eventObject = resolvedEvent.Event.ToEventObject();
        Type publisherType = typeof(IPublishEvent<>).MakeGenericType(eventObject.GetType());
        using (Container.BeginExecutionContextScope()) {
          dynamic publisher = Container.GetInstance(publisherType);
          await publisher.PublishAsync((dynamic)eventObject).ConfigureAwait(true);
        }
        subscription.Acknowledge(resolvedEvent);
      } catch (Exception ex) {
        subscription.Fail(resolvedEvent, PersistentSubscriptionNakEventAction.Unknown, ex.Message);
      }
    }
  }
}
