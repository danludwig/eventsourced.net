using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Packaging;

namespace EventSourced.Net.WebApi
{
  public class Startup
  {
    private readonly Container _container;
    private readonly IConfigurationRoot _configuration;

    public Startup(IHostingEnvironment hostEnv, IApplicationEnvironment appEnv) {
      _container = new Container();

      if (hostEnv.IsDevelopment()) {
        Services.Storage.EventStore.GetEventStore.EnsureRunning(appEnv.ApplicationBasePath);
      }

      IConfigurationBuilder builder = new ConfigurationBuilder()
        .SetBasePath(appEnv.ApplicationBasePath)
        .AddJsonFile("App_Data/Configurations/EventStoreConnection.json")
        .AddEnvironmentVariables();
      _configuration = builder.Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      services
        .AddSingleton(x => _container)
        .AddInstance<IControllerActivator>(new Services.Web.SimpleInjectorControllerActivator(_container))
        .AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory) {
      loggerFactory.AddDebug();
      ComposeRoot();
      app.UseIISPlatformHandler();
      app.UseStaticFiles();
      app.UseMvc();
    }

    private void ComposeRoot() {
      _container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
      var packages = new IPackage[] {

        new Services.Storage.EventStore.Connection.Package(
          _configuration.GetEventStoreConnectionConfiguration()),

        new Services.Messaging.Commands.Package(),
      };
      _container.RegisterPackages(packages);
    }

    // Entry point for the application.
    public static void Main(string[] args) => WebApplication.Run<Startup>(args);
  }
}
