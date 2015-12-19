using System;
using EventStore.ClientAPI;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public interface IConfigureConnection
  {
    string Name { get; }
    System.Net.IPEndPoint GetTcpEndpoint();
    Uri GetUri();
    ConnectionSettings GetConnectionSettings();
    ClusterSettings GetClusterSettings();
  }
}
