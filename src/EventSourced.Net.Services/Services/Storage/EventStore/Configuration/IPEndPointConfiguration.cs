using System.Net;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  // ReSharper disable InconsistentNaming
  public class IPEndPointConfiguration
  // ReSharper restore InconsistentNaming
  {
    public string Address { get; [UsedImplicitly] set; }
    public int Port { get; [UsedImplicitly] set; }

    public static implicit operator IPEndPoint(IPEndPointConfiguration configuration) {
      return configuration != null ? new IPEndPoint(IPAddress.Parse(configuration.Address), configuration.Port) : null;
    }
  }
}