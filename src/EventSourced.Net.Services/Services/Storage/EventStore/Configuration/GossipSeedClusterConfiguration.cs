using System;
using System.Linq;
using EventStore.ClientAPI;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class GossipSeedClusterConfiguration
  {
    public GossipSeedConfiguration[] GossipSeeds { get; [UsedImplicitly] set; }
    public TimeSpan GossipTimeout { get; [UsedImplicitly] set; }
    public int MaxDiscoverAttempts { get; [UsedImplicitly] set; }

    public static implicit operator ClusterSettings(GossipSeedClusterConfiguration configuration) {
      if (configuration == null) return null;
      return ClusterSettings.Create().DiscoverClusterViaGossipSeeds()
        .SetGossipSeedEndPoints(configuration.GossipSeeds.Select(x => (GossipSeed)x).ToArray())
        .SetGossipTimeout(configuration.GossipTimeout)
        .SetMaxDiscoverAttempts(configuration.MaxDiscoverAttempts)
        .Build();
    }
  }
}