using System.Security.Claims;

namespace EventSourced.Net.Web
{
  public class ReduxDataUserState
  {
    public ReduxDataUserState(ClaimsPrincipal user) {
      Username = user.GetUserName();
    }

    public string Username { get; }
  }
}
