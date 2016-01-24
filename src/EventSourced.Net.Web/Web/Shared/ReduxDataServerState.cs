namespace EventSourced.Net.Web
{
  public class ReduxDataServerState
  {
    public ReduxDataServerState() {
      Initialized = true;
    }

    public bool Initialized { get; }
    public bool Unavailable { get; }
  }
}
