using System.Threading.Tasks;

namespace EventSourced.Net
{
  public interface IPublishEvent<TEvent> where TEvent : IEvent
  {
    Task PublishAsync(TEvent @event);
  }
}