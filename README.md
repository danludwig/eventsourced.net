# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

## Up and running
This is an ASP.NET vNext project, currently based on runtime version 1.0.0-rc1-final. Although you won't need to do very much to get it up and running after cloning the repo, you will need the [DotNetVersionManager (`dnvm`)](https://github.com/aspnet/Home/blob/dev/README.md) with [runtime 1.0.0-rc1-final installed](https://github.com/aspnet/Home/wiki/Version-Manager).

Until all of this project's dependent libraries have clr core50-compatible releases, it can only target the `dnx451` framework. This means if you are running it on a Mac, [you will need mono](http://www.mono-project.com/download/) in addition to the dnvm & 1.0.0-rc1-final runtime.
