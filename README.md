# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.



## Up and running
This is an ASP.NET 5 vNext project, currently based on runtime version 1.0.0-rc1-final. Although you won't need to do very much to get it up and running, you will need the [DotNetVersionManager (`dnvm`)](https://github.com/aspnet/Home/blob/dev/README.md#what-you-need) with [runtime 1.0.0-rc1-final installed](https://github.com/aspnet/Home/wiki/Version-Manager#using-the-version-manager) and available in the path.

Until all of this project's dependent libraries have clr core50-compatible releases, it can only target the `dnx451` framework. This means if you are running it on a Mac, [you will need mono](http://www.mono-project.com/download/) in addition to the dnvm & 1.0.0-rc1-final runtime.

### On Windows without Visual Studio
```
# Allow EventStore to run without administrative privileges if you haven't already.
# In either command prompt or powershell **as administrator**:
  # Feel free to replace the user if you want to and you know what you're doing.
  netsh http add urlacl url=http://localhost:2113/ user=everyone
  # If you ever want to reverse this, run the following (also as administrator):
    netsh http delete urlacl url=http://localhost:2113/

# Install the DotNetVersionManager if necessary.
# In either command prompt or powershell:
  dnvm
  # If the prompt tells you dnvm is not recognized, you will need to install it.
  # In powershell **as administrator**:
    # Set your ExecutionPolicy if necessary.
    Get-ExecutionPolicy
    # If the prompt tells you the execution policy is Restricted, change it.
      Set-ExecutionPolicy RemoteSigned
      # When prompted, answer Yes.
      Y
    # Follow the instructions at https://github.com/aspnet/Home/blob/dev/README.md#powershell to install dnvm.
    # Close the administrative powershell window.

    # Run the following in either command prompt or powershell:
    dnvm
    # When prompted, answer Yes.
    Y

# Install & set up runtime version 1.0.0-rc1-final if necessary.
# In either command prompt or powershell:
dnvm install 1.0.0-rc1-final
dnvm use 1.0.0-rc1-final -persistent

# Clone the repository if you haven't already.
# In git bash:
  cd /path/to/local/working/copy/parent # where ever that may be
  git clone https://github.com/danludwig/eventsourced.net.git

# Restore nuget dependencies (if necessary) & start up the web server.
# In either command prompt or powershell:
cd /path/to/local/working/copy/parent/eventsourced.net # where ever that may be
dnu restore
cd src/EventSourced.Net.Web
dnx web

# Make sure you click Yes and Allow access to any prompts about the Network Command Shell and Windows Firewall.
# Wait until two additional command prompts are opened by the dnx command (a total of 3).
# Finally, navigate to http://localhost:5000 in your favorite browser.
```

The very first time you run `dnx web`, the app will automatically download a couple of zip files (containing the database servers), extract them to the `devdbs` folder in your working copy of the repository, then start up each server. How long this takes will depend on your network bandwidth and machine performance, but shouldn't take longer than a minute or two once the zip files are downloaded. If you would like to monitor the progress of this, navigate to the `devdbs` folder which will be created in the root of your working copy of the repository. It should create 2 subfolders under `devdbs`, one for `EventStore` and another for `ArangoDB`. If you delete these folders, the app will recreate them the next time its web server is started.

If you encounter any errors, try running `dnx web` at least one more time before [posting an issue here in GitHub](https://github.com/danludwig/eventsourced.net/issues). There can be race conditions during the very first startup while the databases are set up. Once they are set up, starting the app should be as simple as running `dnx web` from the `src/EventSourced.Net.Web` repository directory.

### On Windows with Visual Studio

### On MacOS
