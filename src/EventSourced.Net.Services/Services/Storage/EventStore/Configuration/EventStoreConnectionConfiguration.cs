using System;
using System.Net;
using EventStore.ClientAPI;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class EventStoreConnectionConfiguration : IConfigureEventStoreConnection
  {
    public string Name { get; [UsedImplicitly] set; }
    public Uri Uri { get; [UsedImplicitly] set; }
    public IPEndPointConfiguration TcpEndPoint { get; [UsedImplicitly] set; }
    public SettingsConfiguration Settings { get; [UsedImplicitly] set; }
    public ClusterConfiguration Cluster { get; [UsedImplicitly] set; }

    Uri IConfigureEventStoreConnection.GetUri() {
      return Uri;
    }

    IPEndPoint IConfigureEventStoreConnection.GetTcpEndpoint() {
      return TcpEndPoint;
    }

    ConnectionSettings IConfigureEventStoreConnection.GetConnectionSettings() {
      return Settings ?? new SettingsConfiguration();
    }

    ClusterSettings IConfigureEventStoreConnection.GetClusterSettings() {
      if (Cluster != null)
        return Cluster.Dns ?? ((ClusterSettings)Cluster.GossipSeed);
      return null;
    }
  }
}
