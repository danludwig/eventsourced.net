using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleInjector;

namespace EventSourced.Net.Services.Messaging.Events
{
  public class ImmediatelyConsistentEventPublisher<TEvent> : IPublishEvent<TEvent> where TEvent : IEvent
  {
    private readonly Container _container;

    public ImmediatelyConsistentEventPublisher(Container container) {
      _container = container;
    }

    public async Task PublishAsync(TEvent e) {
      Type handlerType = typeof(IHandleEvent<>).MakeGenericType(e.GetType());
      IEnumerable<IHandleEvent<TEvent>> handlers = _container.GetAllInstances(handlerType)
        .Cast<IHandleEvent<TEvent>>();
      Task handlerTasks = Task.WhenAll(handlers.Select(x => x.HandleAsync(e)));
      await handlerTasks;
    }
  }
}