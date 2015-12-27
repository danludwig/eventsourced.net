using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleInjector;

namespace EventSourced.Net.Services.Messaging.Events
{
  public class ImmediatelyConsistentEventPublisher : IPublishEvent
  {
    private Container Container { get; }

    public ImmediatelyConsistentEventPublisher(Container container) {
      Container = container;
    }

    public async Task PublishAsync<TEvent>(TEvent e) where TEvent : IEvent {
      Type handlerType = typeof(IHandleEvent<>).MakeGenericType(e.GetType());
      IEnumerable<IHandleEvent<TEvent>> handlers = Container.GetAllInstances(handlerType)
        .Cast<IHandleEvent<TEvent>>();
      Task handlerTasks = Task.WhenAll(handlers.Select(x => x.HandleAsync(e)));
      await handlerTasks;
    }
  }
}