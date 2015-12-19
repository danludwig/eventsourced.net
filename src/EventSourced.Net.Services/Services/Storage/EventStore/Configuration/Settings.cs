using System;
using System.Linq;
using EventStore.ClientAPI;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class Settings
  {
    public bool EnableVerboseLogging { get; [UsedImplicitly] set; } // false by default
    public int? MaxQueueSize { get; [UsedImplicitly] set; } // 5000 by default
    public int? MaxConcurrentItems { get; [UsedImplicitly] set; } // 5000 by default
    public int? MaxRetries { get; [UsedImplicitly] set; } // 10 by default, set to -1 to KeepRetrying in perpetuity
    public int? MaxReconnections { get; [UsedImplicitly] set; } // 10 by default, set to -1 to KeepReconnecting in perpetuity
    public bool FailOnNoServerResponse { get; [UsedImplicitly] set; }  // false by default
    public bool? RequireMaster { get; [UsedImplicitly] set; } // true by default
    public TimeSpan? ReconnectionDelay { get; [UsedImplicitly] set; } // 100 ms by default
    public TimeSpan? OperationTimeout { get; [UsedImplicitly] set; } // 7000 ms by default
    public TimeSpan? OperationTimeoutCheckPeriod { get; [UsedImplicitly] set; } // 1000 ms by default
    public TimeSpan? HeartbeatInterval { get; [UsedImplicitly] set; }  // 750 ms by default
    public TimeSpan? HeartbeatTimeout { get; [UsedImplicitly] set; }  // 1500 ms by default
    public TimeSpan? ClientConnectionTimeout { get; [UsedImplicitly] set; } // 1000 ms by default
    public UserCredential DefaultUserCredentials { get; [UsedImplicitly] set; } // null by default
    public Ssl Ssl { get; [UsedImplicitly] set; } // false by default
    public string ClusterDns { get; [UsedImplicitly] set; } // null by default
    public int? MaxDiscoverAttempts { get; [UsedImplicitly] set; } // 10 by default
    public TimeSpan? GossipTimeout { get; [UsedImplicitly] set; } // 1000 ms by default
    public int? ExternalGossipPort { get; [UsedImplicitly] set; } //30778 by default
    public GossipSeed[] GossipSeeds { get; [UsedImplicitly] set; } // null by default

    public static implicit operator ConnectionSettings(Settings configuration) {
      return configuration.ConfigureSettings().Build();
    }

    private ConnectionSettingsBuilder ConfigureSettings() {
      ConnectionSettingsBuilder builder = ConnectionSettings.Create();

      if (EnableVerboseLogging) builder.EnableVerboseLogging();
      if (MaxQueueSize.HasValue) builder.LimitOperationsQueueTo(MaxQueueSize.Value);
      if (MaxConcurrentItems.HasValue) builder.LimitConcurrentOperationsTo(MaxConcurrentItems.Value);

      if (MaxRetries.HasValue)
        if (MaxRetries.Value == -1) builder.KeepRetrying();
        else builder.LimitRetriesForOperationTo(MaxRetries.Value);

      if (MaxReconnections.HasValue)
        if (MaxReconnections.Value == -1) builder.KeepReconnecting();
        else builder.LimitReconnectionsTo(MaxReconnections.Value);

      if (RequireMaster.HasValue)
        if (RequireMaster.Value) builder.PerformOnMasterOnly();
        else builder.PerformOnAnyNode();

      if (ReconnectionDelay.HasValue) builder.SetReconnectionDelayTo(ReconnectionDelay.Value);
      if (OperationTimeout.HasValue) builder.SetOperationTimeoutTo(OperationTimeout.Value);
      if (OperationTimeoutCheckPeriod.HasValue) builder.SetTimeoutCheckPeriodTo(OperationTimeoutCheckPeriod.Value);

      if (DefaultUserCredentials != null && DefaultUserCredentials.IsValid)
        builder.SetDefaultUserCredentials(DefaultUserCredentials);

      if (Ssl != null && Ssl.IsValid) builder.UseSslConnection(Ssl.TargetHost, Ssl.ValidateServer);

      if (FailOnNoServerResponse) builder.FailOnNoServerResponse();
      if (HeartbeatInterval.HasValue) builder.SetHeartbeatInterval(HeartbeatInterval.Value);
      if (HeartbeatTimeout.HasValue) builder.SetHeartbeatTimeout(HeartbeatTimeout.Value);
      if (ClientConnectionTimeout.HasValue) builder.WithConnectionTimeoutOf(ClientConnectionTimeout.Value);

      if (!string.IsNullOrWhiteSpace(ClusterDns)) builder.SetClusterDns(ClusterDns);
      if (MaxDiscoverAttempts.HasValue) builder.SetMaxDiscoverAttempts(MaxDiscoverAttempts.Value);
      if (GossipTimeout.HasValue) builder.SetGossipTimeout(GossipTimeout.Value);
      if (ExternalGossipPort.HasValue) builder.SetClusterGossipPort(ExternalGossipPort.Value);
      if (GossipSeeds != null) builder.SetGossipSeedEndPoints(GossipSeeds.Select(x => (global::EventStore.ClientAPI.GossipSeed)x).ToArray());

      return builder;
    }
  }
}
