using System.Net;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class GossipSeed : IpEndPoint
  {
    public string HostHeader { get; [UsedImplicitly] set; }

    public static implicit operator global::EventStore.ClientAPI.GossipSeed(GossipSeed configuration) {
      return configuration != null
        ? new global::EventStore.ClientAPI.GossipSeed(
            new IPEndPoint(IPAddress.Parse(configuration.Address), configuration.Port),
              configuration.HostHeader ?? "")
        : null;
    }
  }
}
