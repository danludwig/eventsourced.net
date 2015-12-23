using Microsoft.Extensions.Configuration;

namespace EventSourced.Net
{
  public static class ConfigurationExtensions
  {
    public static T GetConfiguration<T>(this IConfiguration configuration, string section) {
      return configuration.GetSection(section).Get<T>();
    }
  }
}