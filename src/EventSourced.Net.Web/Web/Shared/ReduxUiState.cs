using EventSourced.Net.Web.Users.Register;

namespace EventSourced.Net.Web
{
  public class ReduxUiState
  {
    public object Login { get; set; }
    public object Register { get; set; }
    public object Verify { get; set; }
    public ReduxUiRedeemState Redeem { get; set; }
    public object CheckUsername { get; set; }
  }
}
