using EventStore.ClientAPI;
using System;
using System.Net;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  internal sealed class Factory : IConstructConnection
  {
    private IConfigureConnection Connection { get; }

    public Factory(IConfigureConnection connection) {
      Connection = connection;
    }

    public IEventStoreConnection ConstructConnection() {
      ConnectionSettings settings = Connection.GetConnectionSettings() ?? ConnectionSettings.Default;
      Uri uri = Connection.GetUri();
      IPEndPoint tcpEndpoint = Connection.GetTcpEndpoint() ?? new IPEndPoint(IPAddress.Loopback, 1113);
      ClusterSettings clusterSettings = Connection.GetClusterSettings();
      if (uri != null)
        return EventStoreConnection.Create(settings, uri, Connection.Name);

      return clusterSettings != null
        ? EventStoreConnection.Create(settings, clusterSettings, Connection.Name)
        : EventStoreConnection.Create(settings, tcpEndpoint, Connection.Name);
    }
  }
}
