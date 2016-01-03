using System;

namespace EventSourced.Net
{
  public abstract class Event : IEvent
  {
    protected Event(DateTime happenedOn) {
      HappenedOn = happenedOn;
    }

    public DateTime HappenedOn { get; }
  }
}
