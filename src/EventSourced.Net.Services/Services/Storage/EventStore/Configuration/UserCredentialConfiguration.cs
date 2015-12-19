using EventStore.ClientAPI.SystemData;
using JetBrains.Annotations;

namespace EventSourced.Net.Services.Storage.EventStore.Configuration
{
  public class UserCredentialConfiguration
  {
    public string Username { get; [UsedImplicitly] set; }
    public string Password { get; [UsedImplicitly] set; }
    internal bool IsValid => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

    public static implicit operator UserCredentials(UserCredentialConfiguration configuration) {
      return configuration != null ? new UserCredentials(configuration.Username, configuration.Password) : null;
    }
  }
}