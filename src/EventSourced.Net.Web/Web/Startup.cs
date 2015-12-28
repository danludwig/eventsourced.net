using System.Reflection;
using EventSourced.Net.ReadModel.Users;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
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

    public Startup(IHostingEnvironment hostEnv, IApplicationEnvironment appEnv) {
      Container = new Container();

      if (hostEnv.IsDevelopment()) {
        Services.Storage.EventStore.GetEventStore.EnsureRunning(appEnv.ApplicationBasePath);
      }

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
        .AddMvc()
          .AddJsonOptions(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory) {
      loggerFactory.AddDebug();
      ComposeRoot();
      app.UseIISPlatformHandler();
      app.UseStaticFiles();
      app.UseExecutionContextScope();
      app.UseStatusCodePagesWithReExecute("/errors/{0}");
      app.UseMvc();
    }

    private void ComposeRoot() {
      Container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
      Assembly[] eventHandlerAssemblies = new[]
      {
        typeof(IHandleEvent<>).Assembly,
        typeof(UserView).Assembly,
        GetType().Assembly,
      };
      var packages = new IPackage[] {

        new Services.Storage.EventStore.Connection.Package(
          Configuration.GetEventStoreConnectionConfiguration()),

        new Services.Storage.EventStore.Subscriptions.Package(),

        new Services.Storage.ArangoDb.Package(),

        new Services.Messaging.Commands.Package(),
        new Services.Messaging.Events.Package(eventHandlerAssemblies),
        new Services.Messaging.Queries.Package(),

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
