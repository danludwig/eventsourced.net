namespace EventSourced.Net.Web.Register.CreateLogin
{
  public class PostApiRequest
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
  }
}