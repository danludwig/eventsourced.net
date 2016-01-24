using System.Security.Claims;

namespace EventSourced.Net.Web
{
  public class ReduxDataState
  {
    public ReduxDataState(ClaimsPrincipal user) {
      User = new ReduxDataUserState(user);
      Server = new ReduxDataServerState();
    }

    public ReduxDataUserState User { get; }
    public ReduxDataServerState Server { get; }
  }
}
