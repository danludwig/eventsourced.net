using System.Security.Claims;

namespace EventSourced.Net.Web
{
  public class ReduxAppState
  {
    public ReduxAppState(ClaimsPrincipal user) {
      Ui = new ReduxUiState();
      Data = new ReduxDataState(user);
      Login = new Login.ReduxState(user);
    }

    public ReduxUiState Ui { get; }
    public ReduxDataState Data { get; }
    public Login.ReduxState Login { get;}
    public Register.ReduxState Register { get; set; }
  }
}
