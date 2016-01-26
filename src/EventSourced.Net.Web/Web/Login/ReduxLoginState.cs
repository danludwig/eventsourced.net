﻿using System.Security.Claims;

namespace EventSourced.Net.Web.Login
{
  public class ReduxLoginState
  {
    public ReduxLoginState(ClaimsPrincipal user) {
      ApiCalls = new object[0];
      Username = user.GetUserName();
    }

    public object[] ApiCalls { get; }
    public string Username { get; }
  }
}
