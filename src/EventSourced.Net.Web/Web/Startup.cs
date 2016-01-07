using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Packaging;

namespace EventSourced.Net.Web
{
  public class Startup
  {
    private Container Container { get; }
    private IConfigurationRoot Configuration { get; }

    public Startup(IApplicationEnvironment appEnv) {
      Container = new Container();

      // download & start databases, where possible
      var eventStoreInstallAndRunTask = Task.Factory.StartNew(() => {
        Services.Storage.EventStore.GetEventStore.EnsureRunning(appEnv.ApplicationBasePath);
      });
      var arangoDbInstallOnWindowsTask = Task.Factory.StartNew(() => {
        Services.Storage.ArangoDb.GetArangoDb.EnsureRunningIfPlatformIsWindows(appEnv.ApplicationBasePath);
        Services.Storage.ArangoDb.GetArangoDb.EnsureConfigured(new Services.Storage.ArangoDb.Settings());
      });
      Task.WaitAll(eventStoreInstallAndRunTask, arangoDbInstallOnWindowsTask);

      IConfigurationBuilder builder = new ConfigurationBuilder()
        .SetBasePath(appEnv.ApplicationBasePath)
        .AddJsonFile("App_Data/Configurations/EventStoreConnection.json")
        .AddJsonFile("App_Data/Configurations/WebSocketServer.json")
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      services
        .AddSingleton(x => Container)
        .AddInstance<IControllerActivator>(new Services.Web.Mvc.SimpleInjectorControllerActivator(Container))
        .AddScoped<ISecurityStampValidator, Services.Web.Mvc.SecurityStampValidator>()
        .AddExternalCookieAuthentication()
        .AddMvc()
          .AddJsonOptions(x => {
            x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            x.SerializerSettings.Converters.Add(new StringEnumConverter());
          });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory) {
      loggerFactory.AddDebug();
      ComposeRoot(app.ApplicationServices);
      app
        .UseIISPlatformHandler()
        .UseStaticFiles()
        .UseExecutionContextScope()
        .UseAuthentication()
        .UseStatusCodePagesWithReExecute("/errors/{0}")
        .UseMvc()
      ;
    }

    private void ComposeRoot(IServiceProvider applicationServices) {
      Container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
      Assembly[] eventHandlerAssemblies = {
        typeof(IHandleEvent<>).Assembly,
        typeof(ReadModel.Users.Internal.Handlers.CreateUserDocument).Assembly,
        GetType().Assembly,
      };
      Assembly[] commandHandlerAssemblies = {
        typeof(IHandleCommand<>).Assembly,
        GetType().Assembly,
      };
      Assembly[] queryHandlerAssemblies = {
        typeof(IHandleQuery<,>).Assembly,
        GetType().Assembly,
      };

      var packages = new IPackage[] {

        new Services.Web.Mvc.IdentityOptionsPackage(applicationServices),

        new Services.DataProtection.Package(applicationServices),

        new Services.Storage.EventStore.Connection.Package(
          Configuration.GetEventStoreConnectionConfiguration()),

        new Services.Storage.EventStore.Subscriptions.Package(),

        new Services.Storage.ArangoDb.Package(),

        new Services.Messaging.Commands.Package(commandHandlerAssemblies),
        new Services.Messaging.Events.Package(eventHandlerAssemblies),
        new Services.Messaging.Queries.Package(queryHandlerAssemblies),

        new Services.Web.Sockets.Package(
          Configuration.GetWebSocketServerConfiguration()),
      };
      Container.RegisterPackages(packages);
      Container.Verify(VerificationOption.VerifyAndDiagnose);
    }

    // Entry point for the application.
    public static void Main(string[] args) => WebApplication.Run<Startup>(args);
  }
}
