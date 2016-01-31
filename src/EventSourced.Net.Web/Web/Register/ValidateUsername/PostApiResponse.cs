namespace EventSourced.Net.Web.Register.ValidateUsername
{
  public class PostApiResponse
  {
    public bool IsAvailable => !ReasonInvalid.HasValue;
    public CommandRejectionReason? ReasonInvalid { get; set; }
  }
}
