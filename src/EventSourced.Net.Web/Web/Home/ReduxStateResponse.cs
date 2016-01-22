using System.Security.Claims;
using EventSourced.Net.Web.Users.Register;

namespace EventSourced.Net.Web.Home
{
  public class ReduxStateResponse
  {
    public ReduxStateResponse(ClaimsPrincipal user) {
      Ui = new UiResponse();
      Data = new DataResponse(user);
    }

    public UiResponse Ui { get; }
    public DataResponse Data { get; }
  }

  public class UiResponse
  {
    public RedeemPayload Redeem { get; set; }
  }

  public class DataResponse
  {
    public DataResponse(ClaimsPrincipal user)
    {
      User = new DataUserResponse(user);
      Server = new DataServerResponse();
    }

    public DataUserResponse User { get; }
    public DataServerResponse Server { get; }
  }

  public class DataUserResponse
  {
    public DataUserResponse(ClaimsPrincipal user)
    {
      Username = user.GetUserName();
    }

    public string Username { get; }
  }

  public class DataServerResponse
  {
    public DataServerResponse() {
      Initialized = true;
    }

    public bool Initialized { get; }
    public bool Unavailable { get; }
  }
}
