using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using ArangoDB.Client;
using ArangoDB.Client.Data;

namespace EventSourced.Net.Services.Storage.ArangoDb
{
  public static class GetArangoDb
  {
    private const string WindowsDownloadUrl = "https://www.arangodb.com/repositories/Windows7/x86_64/ArangoDB-2.7.3-win64.zip";
    private const string WindowsExecutableFileName = "arangod.exe";
    private const string InstallPath = "../../devdbs/ArangoDB";
    private static readonly string BinPath = $"{InstallPath}/bin";
    private static bool? _isWindows;
    private static readonly object BlockAllOtherThreads = new object();

    public static void EnsureInstalledIfPlatformIsWindows(string basePath) {
      if (!IsWindows()) return; // must install arango manually on a mac... right?
      basePath = basePath ?? Environment.CurrentDirectory;
      lock (BlockAllOtherThreads) {
        if (IsInstalled(basePath)) return;
        CleanInstallPath(basePath);
        EnsureInstallPath(basePath);
        string compressedPath = Download(basePath);
        Install(compressedPath, basePath);
      }
    }

    private static bool IsInstalled(string basePath) {
      string executableFileName = WindowsExecutableFileName;
      string filePath = Path.GetFullPath(Path.Combine(basePath, $"{BinPath}/{executableFileName}"));
      bool isInstalled = File.Exists(filePath);
      return isInstalled;
    }

    private static void CleanInstallPath(string basePath) {
      string pathToClean = Path.GetFullPath(Path.Combine(basePath, InstallPath));
      if (Directory.Exists(pathToClean)) {
        Directory.Delete(pathToClean, true);
      }
    }

    private static void EnsureInstallPath(string basePath) {
      string[] foldersToEnsure = InstallPath.Split('/');
      IList<string> ups = foldersToEnsure.Where(x => x == "..").ToList();
      foldersToEnsure = foldersToEnsure.Where(x => !ups.Contains(x))
        .Take(foldersToEnsure.Length - 1).ToArray();
      if (ups.Any()) {
        ups.Add(foldersToEnsure[0]);
        foldersToEnsure[0] = Path.Combine(ups.ToArray());
      }
      var ensuredFolders = new List<string>();
      foreach (var folderToEnsure in foldersToEnsure) {
        ensuredFolders.Add(folderToEnsure);
        string relativePath = Path.Combine(ensuredFolders.ToArray());
        string folderPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
        if (!Directory.Exists(folderPath)) {
          Directory.CreateDirectory(folderPath);
        }
      }
    }

    private static string Download(string basePath) {
      var downloadUri = new Uri(WindowsDownloadUrl);
      var downloadFile = downloadUri.ToString().Split('/').Last();
      var downloadPath = Path.GetFullPath(Path.Combine(basePath, $"../../devdbs/{downloadFile}"));
      if (!File.Exists(downloadPath)) {
        using (var webClient = new WebClient()) {
          webClient.DownloadFile(downloadUri, downloadPath);
        }
      }
      return downloadPath;
    }

    private static void Install(string compressedPath, string basePath) {
      if (compressedPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)) {
        string extractPath = Path.GetFullPath(Path.Combine(basePath, $"{InstallPath}Temp"));
        ZipFile.ExtractToDirectory(compressedPath, extractPath);
        string extractedFolderName = compressedPath.Split('\\').Last().Replace(".zip", "");
        string extractedPath = Path.GetFullPath(Path.Combine(extractPath, extractedFolderName));
        if (Directory.Exists(extractedPath)) {
          string installPath = Path.GetFullPath(Path.Combine(basePath, InstallPath));
          Directory.Delete(installPath, true);
          Directory.Move(extractedPath, installPath);
          Directory.Delete(extractPath, true);
          File.Delete(compressedPath);
        }
      } else {
        throw new NotSupportedException($"Unable to install EventStore from compressed path '{compressedPath}'.");
      }
    }

    public static void EnsureRunningIfPlatformIsWindows(string basePath) {
      if (!IsWindows()) return; // must start arango manually on a mac... right?
      EnsureInstalledIfPlatformIsWindows(basePath);
      if (IsRunning()) return;

      var process = new Process {
        StartInfo = new ProcessStartInfo {
          WorkingDirectory = Path.GetFullPath(Path.Combine(basePath, BinPath)),
          FileName = WindowsExecutableFileName,
          UseShellExecute = true,
          CreateNoWindow = false,
        },
      };

      process.Start();
    }

    private static bool IsRunning() {
      string processName = "arangod";
      IEnumerable<Process> arangoProcesses = Process.GetProcesses().Where(x => {
        try {
          return processName.Equals(x.ProcessName, StringComparison.OrdinalIgnoreCase);
        } catch {
          return false;
        }
      });
      return arangoProcesses.Any();
    }

    public static void EnsureConfigured(Settings settings) {
      const int waitForStartupSeconds = 5;
      var waitForStartup = TimeSpan.FromSeconds(waitForStartupSeconds);
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      while (!IsRunning()) {
        System.Threading.Thread.Sleep(1000);
        if (stopwatch.Elapsed >= waitForStartup)
          throw new ApplicationException($"Waited too long (over {waitForStartupSeconds} seconds) for ArangoDb to start up. " +
            "If you are on a Mac, make sure to install the ArangoDB app from the App Store and startup a local instance " +
            $"at port {settings.ServerUri.Port}. " +
            "See the readme for more information https://github.com/danludwig/eventsourced.net");
      }
      while (true) {
        try {
          using (var systemDb = new ArangoDatabase(settings.ServerUrl, "_system")) {
            List<string> dbs = systemDb.ListDatabases();
            if (!dbs.Contains(settings.DbName)) {
              systemDb.CreateDatabase(settings.DbName);
            }
            using (var appDb = new ArangoDatabase(settings.ServerUrl, settings.DbName)) {
              List<CreateCollectionResult> collections = appDb.ListCollections();
              string[] collectionsNeeded = {
                typeof(ReadModel.Users.Internal.Documents.UserDocument).Name,
                typeof(ReadModel.Users.Internal.Documents.UserLoginIndex).Name,
              };
              foreach (string collectionNeeded in collectionsNeeded) {
                if (!collections.Any(x => collectionNeeded.Equals(x.Name))) {
                  appDb.CreateCollection(collectionNeeded);
                }
              }
            }
            break;
          }
        } catch (System.Net.Http.HttpRequestException ex)
          when (ex.InnerException?.GetType() == typeof(WebException)
            && ex.InnerException?.Message == "Unable to connect to the remote server") {
          if (stopwatch.Elapsed > waitForStartup) {
            var throwEx = new ApplicationException($"Could not connect to ArangoDB database at {settings.ServerUrl}. " +
              "Start ArangoDB if it is not running, otherwise check the port. " +
              "If you are on a Mac, the Arango app likes to create instances at port 8000 instead of 8529. " +
              "See the readme for more information https://github.com/danludwig/eventsourced.net", ex);
            throw throwEx;
          }
          //} catch (Exception ex) {
          //  throw;
        }
      }
      stopwatch.Stop();
    }

    private static bool IsWindows() {
      if (!_isWindows.HasValue) {
        _isWindows = Environment.OSVersion.VersionString.IndexOf("windows", StringComparison.OrdinalIgnoreCase) >= 0;
      }
      return _isWindows.Value;
    }
  }
}
