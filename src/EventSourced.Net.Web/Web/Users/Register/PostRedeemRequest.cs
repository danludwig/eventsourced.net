namespace EventSourced.Net.Web.Users.Register
{
  public class PostRedeemRequest
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
  }
}