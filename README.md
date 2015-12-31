# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

## Up and running
This is an ASP.NET 5 vNext project, currently based on runtime version 1.0.0-rc1-final. Although you won't need to do very much to get it up and running after cloning the repo, you will need the [DotNetVersionManager (`dnvm`)](https://github.com/aspnet/Home/blob/dev/README.md#what-you-need) with [runtime 1.0.0-rc1-final installed](https://github.com/aspnet/Home/wiki/Version-Manager#using-the-version-manager).

Until all of this project's dependent libraries have clr core50-compatible releases, it can only target the `dnx451` framework. This means if you are running it on a Mac, [you will need mono](http://www.mono-project.com/download/) in addition to the dnvm & 1.0.0-rc1-final runtime.

### On Windows without Visual Studio
1. Clone this repository (do this using **git bash**, *not* Command Prompt or Powershell)<br/>
`cd ~/Code` (or whichever folder you want to contain the working copy folder for this repository)<br/>
`git clone https://github.com/danludwig/eventsourced.net.git`
1. Next, [make sure you have the DotNetVersionManager installed](https://github.com/aspnet/Home/blob/dev/README.md#upgrading-dnvm-or-running-without-visual-studio) (do this and the rest fo the steps using **Command Prompt** or **Powershell**, *not* git bash)<br/>
`dnvm help` (you should see dnvm usage help information after running this command)
1. Install runtime version 1.0.0-rc1-final<br/>
`dnvm install 1.0.0-rc1-final`<br>
Note that if you close your command prompt or powershell window, `dnvm` may try to use a different runtime for the next command prompt or powershell window that you open. You can make it always use runtime version 1.0.0-rc1-final by running the following command. For more information, [read the dnvm manual](https://github.com/aspnet/Home/wiki/Version-Manager#using-the-version-manager).<br/>
`dnvm use 1.0.0-rc1-final -p`
1. Navigate to the cloned repository folder and restore nuget package dependencies<br/>
`cd ~/Code/eventsourced.net`<br/>
`dnu restore` (this step may take a minute or two, so be patient)
1. Navigate to the web project folder and start the web server<br/>
`cd ~/Code/eventsourced.net/src/EventSourced.Net.Web`<br/>
`dnx web`<br/>
1. Open your favorite web browser, navigate to `http://localhost:5000`, and be patient. During the very first run, the app will automatically download, "install", start & configure 2 databases. How long this takes will depend on your network bandwidth and the performance of your machine. If you would like to monitor the progress of this, navigate to the `devdbs` folder which will be created in the root of your working copy of the repository. It should create 2 subfolders under `devdbs`, one for `EventStore` and another for `ArangoDB`. (Note that I use the term "install" loosely here; the databases are downloaded as compressed zip files, and "installing" them is simply a matter of extracting the downloaded zip files. If you would like to "uninstall" the databases, just delete the `devdbs` folder.)
1. If you encounter any errors, try running `dnx web` at least one more time before [posting an issue here in GitHub](https://github.com/danludwig/eventsourced.net/issues). There can be race conditions during the very first startup while the databases are set up. Once they are set up, starting the app should be as simple as running `dnx web` from the `src/EventSourced.Net.Web` repository directory.


### On Windows with Visual Studio

### On MacOS
