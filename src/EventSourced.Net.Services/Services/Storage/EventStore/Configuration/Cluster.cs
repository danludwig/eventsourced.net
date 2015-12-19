using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class Cluster
  {
    public DnsCluster Dns { get; [UsedImplicitly] set; }
    public GossipSeedCluster GossipSeed { get; [UsedImplicitly] set; }
  }
}
