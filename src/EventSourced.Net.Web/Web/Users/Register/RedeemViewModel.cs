using System;

namespace EventSourced.Net.Web.Users.Register
{
  public class RedeemViewModel
  {
    public Guid CorrelationId { get; set; }
    public string Token { get; set; }
    public string ContactValue { get; set; }
  }
}