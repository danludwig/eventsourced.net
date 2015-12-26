using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EventSourced.Net
{
  public static class ReflectionExtensions
  {
    public static string GetManifestResourceText(this Assembly assembly, string resourceName) {
      using (var stream = assembly.GetManifestResourceStream(resourceName)) {
        if (stream == null) throw new InvalidOperationException(
          $"Unable to get stream for embedded resource '{resourceName}' in assembly '{assembly.FullName}'.");

        using (var reader = new StreamReader(stream)) {
          return reader.ReadToEnd();
        }
      }
    }

    public static string GetManifestResourceName(this Assembly assembly, string resourceName) {
      var allNames = assembly.GetManifestResourceNames();
      return allNames.Single(x => x.Contains(resourceName));
    }
  }
}
