namespace EventSourced.Net.Web.Users.Register
{
  public class UsernameCheckResponseModel
  {
    public bool IsAvailable => !ReasonInvalid.HasValue;
    public CommandRejectionReason? ReasonInvalid { get; set; }
  }
}