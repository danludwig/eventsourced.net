using System;
using System.Net;
using EventStore.ClientAPI;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public interface IConfigureEventStoreConnection
  {
    string Name { get; }
    IPEndPoint GetTcpEndpoint();
    Uri GetUri();
    ConnectionSettings GetConnectionSettings();
    ClusterSettings GetClusterSettings();
  }
}