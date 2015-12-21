using System;
using EventSourced.Net.Services.Storage.EventStore.Connection;
using EventStore.ClientAPI;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class Connection : IConfigureConnection
  {
    public string Name { get; [UsedImplicitly] set; }
    public Uri Uri { get; [UsedImplicitly] set; }
    public IpEndPoint TcpEndPoint { get; [UsedImplicitly] set; }
    public Settings Settings { get; [UsedImplicitly] set; }
    public Cluster Cluster { get; [UsedImplicitly] set; }

    Uri IConfigureConnection.GetUri() {
      return Uri;
    }

    System.Net.IPEndPoint IConfigureConnection.GetTcpEndpoint() {
      return TcpEndPoint;
    }

    ConnectionSettings IConfigureConnection.GetConnectionSettings() {
      return Settings ?? new Settings();
    }

    ClusterSettings IConfigureConnection.GetClusterSettings() {
      if (Cluster != null)
        return Cluster.Dns ?? ((ClusterSettings)Cluster.GossipSeed);
      return null;
    }
  }
}
