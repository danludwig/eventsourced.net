namespace EventSourced.Net.Web.Users.Register
{
  public class PostCheckUsernameResponse
  {
    public bool IsAvailable => !ReasonInvalid.HasValue;
    public CommandRejectionReason? ReasonInvalid { get; set; }
  }
}
