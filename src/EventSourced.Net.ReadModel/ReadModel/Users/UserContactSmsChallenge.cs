namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactSmsChallenge : UserContactChallenge
  {
    public ulong PhoneNumber { get; set; }
    public string RegionCode { get; set; }
  }
}