using EventStore.ClientAPI;
using System;
using System.Threading.Tasks;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  internal sealed class Provider : IProvideConnection
  {
    private readonly IConstructConnection _factory;
    private IEventStoreConnection _connection;
    private static readonly object LockSync = new object();

    public Provider(IConstructConnection factory) {
      _factory = factory;
    }

    public async Task<IEventStoreConnection> GetConnectionAsync() {
      if (_connection != null) return _connection;

      lock (LockSync) {
        _connection = _factory.ConstructConnection();
      }
      _connection.Closed += OnClosed;
      await _connection.ConnectAsync().ConfigureAwait(false);

      return _connection;
    }

    private void OnClosed(object sender, ClientClosedEventArgs args) {
      _connection = null;
    }

    void IDisposable.Dispose() {
      _connection?.Dispose();
      _connection = null;
    }
  }
}
