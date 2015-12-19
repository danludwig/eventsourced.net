using System;
using EventStore.ClientAPI;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class DnsCluster
  {
    public string ClusterDns { get; [UsedImplicitly] set; }
    public int ExternalGossipPort { get; [UsedImplicitly] set; }
    public TimeSpan GossipTimeout { get; [UsedImplicitly] set; }
    public int MaxDiscoverAttempts { get; [UsedImplicitly] set; }

    public static implicit operator ClusterSettings(DnsCluster configuration) {
      if (configuration == null) return null;
      return ClusterSettings.Create().DiscoverClusterViaDns()
        .SetClusterDns(configuration.ClusterDns)
        .SetClusterGossipPort(configuration.ExternalGossipPort)
        .SetGossipTimeout(configuration.GossipTimeout)
        .SetMaxDiscoverAttempts(configuration.MaxDiscoverAttempts)
        .Build();
    }
  }
}
