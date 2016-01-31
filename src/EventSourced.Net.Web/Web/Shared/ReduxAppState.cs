using System.Security.Claims;

namespace EventSourced.Net.Web
{
  public class ReduxAppState
  {
    public ReduxAppState(ClaimsPrincipal user) {
      Login = new Login.ReduxState(user);
    }

    public Login.ReduxState Login { get;}
    public Register.ReduxState Register { get; set; }
  }
}
