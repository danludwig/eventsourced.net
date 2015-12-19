using System.Net;
using EventStore.ClientAPI;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class GossipSeedConfiguration : IPEndPointConfiguration
  {
    public string HostHeader { get; [UsedImplicitly] set; }

    public static implicit operator GossipSeed(GossipSeedConfiguration configuration) {
      return configuration != null
        ? new GossipSeed(new IPEndPoint(IPAddress.Parse(configuration.Address), configuration.Port),
          configuration.HostHeader ?? "")
        : null;
    }
  }
}