using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace EventSourced.Net.Services.Storage.EventStore
{
  public static class GetEventStore
  {
    private const string WindowsDownloadUrl = "http://download.geteventstore.com/binaries/EventStore-OSS-Win-v3.3.1.zip";
    private const string WindowsExecutableFileName = "EventStore.ClusterNode.exe";
    private const string MacDownloadUrl = "http://download.geteventstore.com/binaries/EventStore-OSS-MacOSX-v3.3.1.tar.gz";
    private const string MacExecutableFileName = "run-node.sh";
    private const string InstallPath = "../../devdbs/EventStore";
    private static bool? _isWindows;
    private static readonly object BlockAllOtherThreads = new object();

    public static void EnsureInstalled(string basePath) {
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
      string executableFileName = IsWindows() ? WindowsExecutableFileName : MacExecutableFileName;
      string filePath = Path.GetFullPath(Path.Combine(basePath, $"{InstallPath}/{executableFileName}"));
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
      var downloadUri = new Uri(IsWindows() ? WindowsDownloadUrl : MacDownloadUrl);
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
        ZipFile.ExtractToDirectory(compressedPath,
          Path.GetFullPath(Path.Combine(basePath, InstallPath)));
        File.Delete(compressedPath);

      } else if (compressedPath.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase)) {
        var workingDirectory = Path.GetFullPath(Path.Combine(basePath,
            InstallPath.Substring(0, InstallPath.LastIndexOf("/", StringComparison.Ordinal))));
        var processStartInfo = new ProcessStartInfo {
          WorkingDirectory = workingDirectory,
          FileName = "gunzip",
          Arguments = $"-d {compressedPath.Split('/').Last()}"
        };
        using (var process = new Process { StartInfo = processStartInfo, }) {
          process.Start();
          process.WaitForExit();
        }

        compressedPath = compressedPath.Substring(0, compressedPath.Length - 3);
        processStartInfo.FileName = "tar";
        processStartInfo.Arguments = $"-xf {compressedPath.Split('/').Last()}";
        using (var process = new Process { StartInfo = processStartInfo, }) {
          process.Start();
          process.WaitForExit();
        }
        if (File.Exists(compressedPath)) File.Delete(compressedPath);

        compressedPath = compressedPath.Substring(0, compressedPath.Length - 4);
        string installPath = Path.GetFullPath(Path.Combine(basePath, InstallPath));
        if (Directory.Exists(installPath)) Directory.Delete(installPath);
        Directory.Move(compressedPath, installPath);

      } else {
        throw new NotSupportedException($"Unable to install EventStore from compressed path '{compressedPath}'.");
      }
    }

    public static void EnsureRunning(string basePath) {
      EnsureInstalled(basePath);
      if (IsRunning()) return;

      var args = new List<string>
      {
          "--db ./db",
          "--log ./logs",
          "--run-projections=system",
          "--start-standard-projections",
        };
      var isWindows = IsWindows();
      if (!isWindows) {
        args.Insert(0, "-a");
        args.Insert(1, "Terminal.app");
        args.Insert(2, MacExecutableFileName);
        args.Insert(3, "--args");
      }

      var process = new Process {
        StartInfo = new ProcessStartInfo {
          FileName = isWindows ? WindowsExecutableFileName : "open",
          WorkingDirectory = Path.GetFullPath(Path.Combine(basePath, InstallPath)),
          Arguments = args.Aggregate((current, next) => $"{current} {next}"),
          UseShellExecute = true,
          CreateNoWindow = false,
        },
      };

      process.Start();
    }

    private static bool IsRunning() {
      string processName = IsWindows() ? "EventStore.ClusterNode" : "eventstored";
      IEnumerable<Process> eventStoreProcesses = Process.GetProcesses().Where(x => {
        try {
          return processName.Equals(x.ProcessName, StringComparison.OrdinalIgnoreCase);
        } catch {
          return false;
        }
      });
      return eventStoreProcesses.Any();
    }

    private static bool IsWindows() {
      if (!_isWindows.HasValue) {
        _isWindows = Environment.OSVersion.VersionString.IndexOf("windows", StringComparison.OrdinalIgnoreCase) >= 0;
      }
      return _isWindows.Value;
    }
  }
}
