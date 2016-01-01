# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

## Up and running

When you're ready to clone the repository:

    cd /path/to/local/working/copy/parent # where ever that may be
    git clone https://github.com/danludwig/eventsourced.net.git
    cd eventsourced.net

### On Windows

This app uses the  [EventStore database](https://geteventstore.com/), which when not run as an administrator (or started up from another program running as administrator) will likely encounter an error while trying to start its HTTP server at  [http://localhost:2113](http://localhost:2113). After running the following command once as administrator, you should be able to start EventStore normally without experiencing this error:

    # Feel free to replace the user if you want to and you know what you're doing.
    netsh http add urlacl url=http://localhost:2113/ user=everyone

If you ever want to undo the above command, run this (also once as administrator):

    netsh http delete urlacl url=http://localhost:2113/

#### With Visual Studio



#### Without Visual Studio
##### .NET Version Manager
Run the following in either command prompt or powershell to check if the .NET Version Manager is installed:

    dnvm

If dnvm is not recognized, you should [install it by running this command **in powershell**](https://github.com/aspnet/Home/blob/dev/README.md#powershell).

If you encounter an error while running the install command, check `Get-ExecutionPolicy`. If it is `Restricted`, start a new powershell window *as administrator* and change it using `Set-ExecutionPolicy RemoteSigned`. Repeating the dnvm powershell install command should then succeed. If you'd like to, use `Set-ExecutionPolicy Restricted` to restore the previous execution policy after installation is complete.

After installing you should close the installer window, open a new command prompt or powershell window, and run the `dnvm` command again to confirm that it has been installed.

##### Runtime 1.0.0-rc1-final

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
