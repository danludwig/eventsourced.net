﻿using EventSourced.Net.Services.Storage.EventStore;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace EventSourced.Net.WebApi
{
  public class Startup
  {
    public Startup(IHostingEnvironment hostEnv, IApplicationEnvironment appEnv) {
      if (hostEnv.IsDevelopment()) {
        GetEventStore.EnsureRunning(appEnv.ApplicationBasePath);
      }
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app) {
      app.UseIISPlatformHandler();

      app.Run(async (context) => {
        await context.Response.WriteAsync("Hello World!");
      });
    }

    // Entry point for the application.
    public static void Main(string[] args) => WebApplication.Run<Startup>(args);
  }
}
