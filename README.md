# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

- [Up and running](#up-and-running)
  - [On Windows](#on-windows)
    - [With Visual Studio](#with-visual-studio)
    - [Without Visual Studio](#without-visual-studio)
  - [On MacOS](#on-macos)

## Up and running

When you're ready to clone the repository:

    cd /path/to/local/working/copy/parent # where ever that may be
    git clone https://github.com/danludwig/eventsourced.net.git
    cd eventsourced.net

### On Windows

This app uses the  [EventStore database](https://geteventstore.com/), which when not run as an administrator (or started up from another program running as administrator) will likely encounter an error while trying to start its HTTP server at  [http://localhost:2113](http://localhost:2113). After running the following command once as administrator, starting EventStore normally should not cause this error:

    # Feel free to change the user if you want to and you know what you're doing.
    netsh http add urlacl url=http://localhost:2113/ user=everyone

If you ever want to undo the above command, run this (also once as administrator):

    netsh http delete urlacl url=http://localhost:2113/

#### With Visual Studio

To run in Visual Studio, you will need at least version 2015 with Update 1 installed. Don't try to open the solution or any of the project files unless you're sure you also have ASP.NET 5 RC installed, as described below.

##### Download & install ASP.NET 5 RC if necessary
- Start Visual Studio.
- Type <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>N</kbd> to create a new project.
- Select `Templates/Visual C#/Web -> ASP.NET Web Application` and click `OK`.
- If you see 3 items under the `ASP.NET 5 Templates` section, cancel out of all dialogs and proceed to the next step.
- If instead you see a single `Get ASP.NET 5 RC` item under the `ASP.NET 5 Templates` section, select it and click OK. This will automatically download an additional exe file that you will need to run in order to install some things. Note you will have to close all instances of Visual Studio for the installer to complete. When finished, repeat the above steps to confirm you can create a new project using one of the 3 `ASP.NET 5 Templates`.

##### Open the solution
The first time you open [this app's solution file](https://github.com/danludwig/eventsourced.net/blob/master/EventSourced.Net.sln), you may be prompted to install a DNX SDK verion ending in `1.0.0-rc1-final`. Be sure to click `Yes` at this prompt. If for some reason the install fails, close the solution, open a command prompt or powershell window, run `dnvm install 1.0.0-rc1-final` and re-open the solution.

##### Build the solution
Keep an eye on the Solution Explorer and wait for it to finish `Restoring packages` if necessary. Once idle, type <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> to build the solution. If for some reason the build fails, open a command prompt or powershell window, navigate to the root of your working copy clone, run `dnu restore` and rebuild the solution.

##### Start the EventSourced.Net.Web project for the first time
- In Solution Explorer right-click `Solution 'EventSourced.Net' (6 projects)` and select `Properties`.
- Make sure the `Single startup project` radio button is selected, the `EventSourced.Net.Web` item is selected in the drop down, and then click `OK`.
- Type <kbd>F5</kbd> to start the app.
- Be patient. There is a lot that happens during the first app run. It will start up much faster next time.
- When prompted about allowing `Network Command Shell` to make changes to your PC, click `Yes`.
- When prompted by the Windows Firewall about allowing dnx.exe to communicate on private networks, click `Allow access`.

#### Without Visual Studio
##### .NET Version Manager
Run the following in either command prompt or powershell to check if the .NET Version Manager is installed:

    dnvm

If dnvm is not recognized, you should [install it by running this command **in powershell**](https://github.com/aspnet/Home/blob/dev/README.md#powershell).

If you encounter an error while running the install command, check `Get-ExecutionPolicy`. If it is `Restricted`, start a new powershell window *as administrator* and change it using `Set-ExecutionPolicy RemoteSigned`. Repeating the dnvm powershell install command should then succeed. If you'd like to, use `Set-ExecutionPolicy Restricted` to restore the previous execution policy after installation is complete.

After installing you should close the installer window, open a new command prompt or powershell window, and run the `dnvm` command again to confirm that it has been installed.

##### Runtime 1.0.0-rc1-final
The .NET Version Manager installer will install the latest runtime and make it the default. This app currently targets runtime `1.0.0-rc1-final`, which you will need to have installed and available on your environment path. To find out which runtime is the default, run the following in command prompt or powershell:

    dnvm list

If you do not see an entry with Version `1.0.0-rc1-final` in the listing, run the following to install it:

    dnvm install 1.0.0-rc1-final

After the installation has finished, run `dnvm list` again in a new command prompt or powershell window. If you see that Version `1.0.0-rc1-final` is no longer the Active version, run the following:

    dnvm use 1.0.0-rc1-final -persistent

Unless otherwise specified, any other documentation about the `dnu` or `dnx` commands in this readme will assume that version `1.0.0-rc1-final` is the currently Active version in the environment path, according to `dnvm list`.

##### Restore nuget package dependencies

The first time you clone the repository, and each time a nuget package dependency is added, removed, or changed, you will need to run the following at the repository root folder in either command prompt or powershell:

    cd /path/to/local/working/copy/parent # where ever that may be
    dnu restore

Note that restoring packages may take a couple of minutes when run for the first time.

##### Launch the app

Run the following in commmand prompt or powershell from the `src/EventSourced.Net.Web` project directory to start the app in a web server:

    cd src/EventSourced.Net.Web
    dnx web

- Be patient. There is a lot that happens during the first app run. It will start up much faster next time.
- When prompted about allowing `Network Command Shell` to make changes to your PC, click `Yes`.
- When prompted by the Windows Firewall about allowing dnx.exe to communicate on private networks, click `Allow access`.
- Finally, navigate to [http://localhost:5000](http://localhost:5000) in your favorite web browser.

### On MacOS

##### .NET Version Manager
The first thing you will need to run this app on a mac is the .NET Version Manager (`dnvm`). To find out if you have it installed already, run the following in a terminal window:

    dnvm

If the reponse tells you that the dnvm command was not found, [download & install the ASP.NET 5 pkg from get.asp.net](https://get.asp.net). After the installation has finished, close the terminal window, open a new one, and run the above command again to confirm that it is installed and available on your environment path.

##### Mono
You will also need Mono, since this app currently does not target the .NET core50 framework. To find out if you have mono installed, run the following in a terminal window:

    mono -V

If the response tells you that the mono command was not found, or reports back a version less than 4.2, [download & install a Mono pkg for version 4.2 or higher](http://www.mono-project.com/download/). After the installation has finished, close the terminal window, open a new one, and run the above command again to confirm that it is installed and available on your environment path.

##### Runtime 1.0.0-rc1-final
The .NET Version Manager installer will install the latest runtime and make it the default. This app currently targets runtime `1.0.0-rc1-final`, which you will need to have installed and available on your environment path. To find out which runtime is the default, run the following in a terminal window:

    dnvm list

If you do not see an entry with Version `1.0.0-rc1-final` & Runtime `mono` in the listing, run the following to install it:

    dnvm install 1.0.0-rc1-final

After the installation has finished, run `dnvm list` again in a *new* teriminal window. If you see that Version `1.0.0-rc1-final` is no longer the Active version, run the following:

    dnvm use 1.0.0-rc1-final -persistent

Unless otherwise specified, any other documentation about the `dnu` or `dnx` commands in this readme will assume that version `1.0.0-rc1-final` is the currently Active version in the environment path, according to `dnvm list`.
