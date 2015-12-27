using System;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public abstract class UserContactChallenge
  {
    public Guid Id { get; set; }
    public ContactChallengePurpose Purpose { get; set; }
  }
}