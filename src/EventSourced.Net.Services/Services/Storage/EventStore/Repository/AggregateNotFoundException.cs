using System;

namespace EventSourced.Net.Services.Storage.EventStore.Repository
{
  public class AggregateNotFoundException : Exception
  {
    public readonly Guid Id;
    public readonly Type Type;

    public AggregateNotFoundException(Guid id, Type type)
      : base($"Aggregate '{id}' (type {type.Name}) was not found.") {
      Id = id;
      Type = type;
    }
  }
}
