namespace EventSourced.Net.Web.Users.Login
{
  public class LoginRequestModel
  {
    public string Login { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
  }
}