# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

## Up and running

```
# Clone the repository if you haven't already.
# In git bash or mac terminal:
  cd /path/to/local/working/copy/parent # where ever that may be
  git clone https://github.com/danludwig/eventsourced.net.git
```

This is an ASP.NET 5 vNext project, currently based on runtime version 1.0.0-rc1-final. Although you won't need to do very much to get it up and running, you will need the [DotNetVersionManager (`dnvm`)](https://github.com/aspnet/Home/blob/dev/README.md#what-you-need) with [runtime 1.0.0-rc1-final installed](https://github.com/aspnet/Home/wiki/Version-Manager#using-the-version-manager) and available in the path. Until all of this project's dependent libraries have clr core50-compatible releases, it can only target the `dnx451` framework. This means if you are running it on a Mac, [you will need mono](http://www.mono-project.com/download/) in addition to the dnvm & 1.0.0-rc1-final runtime.

Note the very first time you start up the app (using either `dnx web`, OmniSharp, or Visual Studio), the app will automatically download a (couple of) compressed file(s) containing the database server(s). On windows both databases will be downloaded, but for MacOS, only one is currently automated. Next, the app will install each database by extracting its compressed file to the `devdbs` folder in your working copy of the repository, then start up each server. How long this takes will depend on your platform, network bandwidth and machine performance, but shouldn't take longer than a minute or two once the zip files are downloaded.

If you would like to monitor the progress of this, navigate to the `devdbs` folder which will be created in the root of your working copy of the repository. On both platforms it should create a subfolder under `devdbs` for `EventStore`, whereas on windows it will also create a subfolder for `ArangoDB`. If you delete these folders, the app will recreate them the next time its web server is started.

If you encounter any errors, try running the app at least one more time before [posting an issue here in GitHub](https://github.com/danludwig/eventsourced.net/issues). There can be race conditions during the very first run while the databases are set up. Once they are set up, starting the app should be as simple as running `dnx web` from the `src/EventSourced.Net.Web` repository directory, or F5 from Visual Studio.

### On Windows

Whether you want to run the app with or without Visual Studio, you probably don't want to have to start things up as administrator all of the time. This app uses a database called [EventStore](https://geteventstore.com/), which when not run under an administrator security context, can encounter an error when trying to start its HTTP server at http://localhost:2113. To work around this, you should add an ACL for this url.

```
# Allow EventStore to run without administrative privileges if you haven't already.
# In either command prompt or powershell **as administrator**:
  # Feel free to replace the user if you want to and you know what you're doing.
  netsh http add urlacl url=http://localhost:2113/ user=everyone
  # If you ever want to reverse this, run the following (also as administrator):
    netsh http delete urlacl url=http://localhost:2113/
```

#### With Visual Studio

To run in Visual Studio, you will need at least version 2015 with Update 1 installed. Visual Studio Update 1 should automatically install the DotNetVersionManager (`dnvm`) for you, but may not install runtime version `1.0.0-rc1-final`. Use the following to make sure you have the correct runtime installed and available in your user path.

```
# Install & set up runtime version 1.0.0-rc1-final if necessary.
# In either command prompt or powershell:
dnvm install 1.0.0-rc1-final
dnvm use 1.0.0-rc1-final -persistent
```

Open the [EventSourced.Net.sln](https://github.com/danludwig/eventsourced.net/blob/master/EventSourced.Net.sln) file in Visual Studio, wait for the projects to load, then select `Build > Rebuild Solution` from the menu bar. You should see some output while Visual Studio uses the `dnu restore` utility in the background to download any missing nuget packages. If for any reason this fails, run `dnu restore` manually to kick Visual Studio back into shape.

```
# Restore nuget dependencies (if necessary).
# In command prompt:
cd /path/to/local/working/copy/parent/eventsourced.net # where ever that may be
dnu restore
```

Finally, hit F5 or select `Debug > Start Without Debugging` from the menu bar to start up the app.

#### Without Visual Studio

```
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

# Restore nuget dependencies (if necessary) & start up the app
# In command prompt:
cd /path/to/local/working/copy/parent/eventsourced.net # where ever that may be
dnu restore
cd src/EventSourced.Net.Web
dnx web

# Make sure you click Yes and Allow access to any prompts about the Network Command Shell and Windows Firewall.
# Wait until two additional command prompts are opened by the dnx command (a total of 3).
# Finally, navigate to http://localhost:5000 in your favorite browser.
```

To stop the app, type CTRL+C in each of the 3 command prompts until their processes are stopped.

### On MacOS

The first thing you will need to run this app on a mac is the DotNetVersionManager (`dnvm`). To find out if you have it installed already, run the following in a Terminal window:
```
dnvm
```
If the reponse tells you that the dnvm command was not found, [download & install the ASP.NET 5 pkg from get.asp.net](http://get.asp.net).

You will also need Mono, since this app currently does not target the ASP.NET core50 framework. To find out if you have mono installed, run the following in a Terminal window:
```
mono -V
```
If the response tells you that the mono command was not found, or reports back a version less than 4.2, [download & install a mono pkg for version 4.2 or higher](http://www.mono-project.com/download/).
