namespace EventSourced.Net.Services.Web.Sockets
{
  public class ServerSettings
  {
    public static readonly ServerSettings Default = new ServerSettings(
      new ServerConfiguration {
        Port = 3000,
      });

    internal ServerSettings(ServerConfiguration configuration) {
      Port = configuration?.Port ?? Default.Port;
    }

    public int Port { get; }
  }
}
