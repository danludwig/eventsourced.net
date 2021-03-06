﻿using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class Ssl
  {
    public Ssl() {
      ValidateServer = true;
    }

    public string TargetHost { get; [UsedImplicitly] set; }
    public bool ValidateServer { get; [UsedImplicitly] set; }
    internal bool IsValid => !string.IsNullOrEmpty(TargetHost);
  }
}
