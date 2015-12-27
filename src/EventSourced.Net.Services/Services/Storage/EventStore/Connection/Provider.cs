using EventStore.ClientAPI;
using System;
using System.Threading.Tasks;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  internal sealed class Provider : IProvideConnection
  {
    private IConstructConnection Factory { get; }
    private IEventStoreConnection Connection { get; set; }
    private static readonly object BlockAllOtherThreads = new object();

    public Provider(IConstructConnection factory) {
      Factory = factory;
    }

    public async Task<IEventStoreConnection> GetConnectionAsync() {
      if (Connection != null) return Connection;

      lock (BlockAllOtherThreads) {
        Connection = Factory.ConstructConnection();
      }
      Connection.Closed += OnClosed;
      await Connection.ConnectAsync().ConfigureAwait(false);

      return Connection;
    }

    private void OnClosed(object sender, ClientClosedEventArgs args) {
      Connection = null;
    }

    void IDisposable.Dispose() {
      Connection?.Dispose();
      Connection = null;
    }
  }
}
