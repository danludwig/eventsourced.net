namespace EventSourced.Net
{
  public interface IHandleEvent<in TEvent> : IHandleMessage<TEvent> where TEvent : IEvent { }
}