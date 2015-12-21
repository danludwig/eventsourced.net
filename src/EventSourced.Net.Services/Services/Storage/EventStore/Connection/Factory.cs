using EventStore.ClientAPI;
using System;
using System.Net;

namespace EventSourced.Net.Services.Storage.EventStore.Connection
{
  public class Factory : IConstructConnection
  {
    private readonly IConfigureConnection _connection;

    public Factory(IConfigureConnection connection) {
      _connection = connection;
    }

    public IEventStoreConnection ConstructConnection() {
      ConnectionSettings settings = _connection.GetConnectionSettings() ?? ConnectionSettings.Default;
      Uri uri = _connection.GetUri();
      IPEndPoint tcpEndpoint = _connection.GetTcpEndpoint() ?? new IPEndPoint(IPAddress.Loopback, 1113);
      ClusterSettings clusterSettings = _connection.GetClusterSettings();
      if (uri != null)
        return EventStoreConnection.Create(settings, uri, _connection.Name);

      return clusterSettings != null
        ? EventStoreConnection.Create(settings, clusterSettings, _connection.Name)
        : EventStoreConnection.Create(settings, tcpEndpoint, _connection.Name);
    }
  }
}
