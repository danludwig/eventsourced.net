using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class ClusterConfiguration
  {
    public DnsClusterConfiguration Dns { get; [UsedImplicitly] set; }
    public GossipSeedClusterConfiguration GossipSeed { get; [UsedImplicitly] set; }
  }
}