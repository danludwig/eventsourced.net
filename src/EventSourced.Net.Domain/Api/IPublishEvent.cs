using System.Threading.Tasks;

namespace EventSourced.Net
{
  public interface IPublishEvent
  {
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
  }
}
