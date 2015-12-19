using System;

namespace EventSourced.Net.Services.Storage.EventStore.Repository
{
  public class AggregateDeletedException : Exception
  {
    public readonly Guid Id;
    public readonly Type Type;

    public AggregateDeletedException(Guid id, Type type)
      : base($"Aggregate '{id}' (type {type.Name}) was deleted.") {
      Id = id;
      Type = type;
    }
  }
}
