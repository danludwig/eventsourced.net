using System.Security.Claims;

namespace EventSourced.Net.Web
{
  public class ReduxAppState
  {
    public ReduxAppState(ClaimsPrincipal user) {
      Ui = new ReduxUiState();
      Data = new ReduxDataState(user);
    }

    public ReduxUiState Ui { get; }
    public ReduxDataState Data { get; }
  }
}
