using EventSourced.Net.ReadModel.Users;

namespace EventSourced.Net.Web.Register.CreateLogin
{
  public class ReduxState
  {
    public ReduxState(UserContactChallengeRedeemData data) {
      ViewData = new ReduxViewData(data);
    }

    public ReduxViewData ViewData { get; }
  }
}
