namespace EventSourced.Net.Services.Web.Sockets
{
  public class ServerConfiguration
  {
    public static implicit operator ServerSettings(ServerConfiguration configuration) {
      return configuration != null ? new ServerSettings(configuration) : null;
    }

    public int? Port { get; set; }
  }
}
