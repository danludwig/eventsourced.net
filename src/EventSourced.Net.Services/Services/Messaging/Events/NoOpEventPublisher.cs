using System.Threading.Tasks;

namespace EventSourced.Net.Services.Messaging.Events
{
  public class NoOpEventPublisher<TEvent> : IPublishEvent<TEvent> where TEvent : IEvent
  {
    public Task PublishAsync(TEvent e) {
      return Task.FromResult(true);
    }
  }
}
