using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  public interface IProvideConnection : IDisposable
  {
    Task<IEventStoreConnection> GetConnectionAsync();
  }
}
