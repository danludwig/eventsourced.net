using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonDomain.Core
{
  public abstract class AggregateBase : IAggregate, IEquatable<IAggregate>
  {
    private readonly ICollection<object> _uncommittedEvents = new LinkedList<object>();
    private IRouteEvents _registeredRoutes;

    public Guid Id { get; protected set; }

    public int Version { get; protected set; }

    protected IRouteEvents RegisteredRoutes {
      get {
        return _registeredRoutes ?? (_registeredRoutes = new ConventionEventRouter(true, this));
      }
      set {
        if (value == null)
          throw new InvalidOperationException("AggregateBase must have an event router to function");
        _registeredRoutes = value;
      }
    }

    protected AggregateBase(IRouteEvents handler = null) {
      if (handler == null)
        return;
      RegisteredRoutes = handler;
      RegisteredRoutes.Register(this);
    }

    protected void Register<T>(Action<T> route) {
      RegisteredRoutes.Register(route);
    }

    protected void RaiseEvent(object @event) {
      ((IAggregate)this).ApplyEvent(@event);
      _uncommittedEvents.Add(@event);
    }

    void IAggregate.ApplyEvent(object @event) {
      RegisteredRoutes.Dispatch(@event);
      Version++;
    }

    ICollection IAggregate.GetUncommittedEvents() {
      return (ICollection)_uncommittedEvents;
    }

    void IAggregate.ClearUncommittedEvents() {
      _uncommittedEvents.Clear();
    }

    IMemento IAggregate.GetSnapshot() {
      IMemento snapshot = GetSnapshot();
      snapshot.Id = Id;
      snapshot.Version = Version;
      return snapshot;
    }

    protected virtual IMemento GetSnapshot() {
      return null;
    }

    public override int GetHashCode() {
      return ((IAggregate)this).Id.GetHashCode();
    }

    public override bool Equals(object obj) {
      return Equals(obj as IAggregate);
    }

    public virtual bool Equals(IAggregate other) {
      if (other != null)
        return other.Id == Id;
      return false;
    }
  }
}
