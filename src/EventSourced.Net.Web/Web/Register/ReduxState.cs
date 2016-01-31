using EventSourced.Net.ReadModel.Users;

namespace EventSourced.Net.Web.Register
{
  public class ReduxState
  {
    public ReduxState(UserContactChallengeRedeemData data) {
      CreateLogin = new CreateLogin.ReduxState(data);
    }

    public CreateLogin.ReduxState CreateLogin { get; }
  }
}