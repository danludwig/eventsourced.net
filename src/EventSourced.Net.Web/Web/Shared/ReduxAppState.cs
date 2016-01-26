using System.Security.Claims;
using EventSourced.Net.Web.Login;

namespace EventSourced.Net.Web
{
  public class ReduxAppState
  {
    public ReduxAppState(ClaimsPrincipal user) {
      Ui = new ReduxUiState();
      Data = new ReduxDataState(user);
      Login = new ReduxLoginState(user);
    }

    public ReduxUiState Ui { get; }
    public ReduxDataState Data { get; }
    public ReduxLoginState Login { get;}
  }
}
