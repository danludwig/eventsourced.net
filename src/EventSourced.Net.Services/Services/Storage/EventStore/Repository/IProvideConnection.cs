using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourced.Net.Services.Storage.EventStore.Repository
{
  public interface IProvideConnection : IDisposable
  {
    Task<IEventStoreConnection> GetConnectionAsync();
  }
}
