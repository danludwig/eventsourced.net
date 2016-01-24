using EventSourced.Net.ReadModel.Users;

namespace EventSourced.Net.Web.Users.Register
{
  public class ReduxUiRedeemState
  {
    public ReduxUiRedeemState(UserContactChallengeRedeemData data) {
      Data = new ReduxUiRedeemData(data);
    }

    public ReduxUiRedeemData Data { get; }
  }
}