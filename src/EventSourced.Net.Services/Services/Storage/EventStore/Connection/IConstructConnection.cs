using EventStore.ClientAPI;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  public interface IConstructConnection
  {
    IEventStoreConnection ConstructConnection();
  }
}