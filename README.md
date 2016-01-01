# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

## Up and running

When you're ready to clone the repository:

    cd /path/to/local/working/copy/parent # where ever that may be
    git clone https://github.com/danludwig/eventsourced.net.git
    cd eventsourced.net

### On Windows

This app uses the  [EventStore database](https://geteventstore.com/), which when not run as an administrator (or started up from another program running as administrator) will likely encounter an error when trying to start its HTTP server at  [http://localhost:2113](http://localhost:2113). After running the following command once as administrator, you should no longer experience this error when EventStore is started without administrative privileges:

    # Feel free to replace the user if you want to and you know what you're doing.
    netsh http add urlacl url=http://localhost:2113/ user=everyone

If you ever want to undo the above command, run this (also once as administrator):

    netsh http delete urlacl url=http://localhost:2113/

#### With Visual Studio

To run in Visual Studio, you will need at least version 2015 with Update 1 installed. Visual Studio Update 1 should automatically install the .NET Version Manager (`dnvm`) for you, but may not install runtime version `1.0.0-rc1-final`. Use the following to make sure you have the correct runtime installed and available in your user path.

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
# Install the .NET Version Manager if necessary.
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

#### .NET Version Manager
The first thing you will need to run this app on a mac is the .NET Version Manager (`dnvm`). To find out if you have it installed already, run the following in a terminal window:

    dnvm

If the reponse tells you that the dnvm command was not found, [download & install the ASP.NET 5 pkg from get.asp.net](http://get.asp.net). After the installation has finished, close the terminal window, open a new one, and run the above command again to confirm that it is installed and available on your environment path.

#### Mono
You will also need Mono, since this app currently does not target the .NET core50 framework. To find out if you have mono installed, run the following in a terminal window:

    mono -V

If the response tells you that the mono command was not found, or reports back a version less than 4.2, [download & install a Mono pkg for version 4.2 or higher](http://www.mono-project.com/download/). After the installation has finished, close the terminal window, open a new one, and run the above command again to confirm that it is installed and available on your environment path.

#### Runtime 1.0.0-rc1-final
The .NET Version Manager installer will install the latest runtime and make it the default. This app currently targets runtime `1.0.0-rc1-final`, which you will need to have installed and available on your environment path. To find out which runtime is the default, run the following in a terminal window:

    dnvm list

If you do not see an entry with Version `1.0.0-rc1-final` & Runtime `mono` in the listing, run the following to install it:

    dnvm install 1.0.0-rc1-final

After the installation has finished, run `dnvm list` again in a *new* teriminal window. If you see that Version `1.0.0-rc1-final` is no longer the Active version, run the following:

    dnvm use 1.0.0-rc1-final -persistent

Unless otherwise specified, any other documentation about the `dnu` or `dnx` commands in this readme will assume that version `1.0.0-rc1-final` is the currently Active version in the environment path, according to `dnvm list`.
