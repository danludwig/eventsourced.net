using EventSourced.Net.Services.Storage.EventStore.Configuration;
using Microsoft.Extensions.Configuration;

namespace EventSourced.Net
{
  public static class EventStoreExtensions
  {
    public static Connection GetEventStoreConnectionConfiguration(this IConfiguration configuration,
      string section = "eventStore:connection") {
      return configuration.GetConfiguration<Connection>(section);
    }
  }
}
