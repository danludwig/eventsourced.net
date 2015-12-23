using System;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.Extensions.Configuration;
using WebSocketSharp;

namespace EventSourced.Net
{
  public static class WebSocketExtensions
  {
    public static ServerConfiguration GetWebSocketServerConfiguration(this IConfiguration configuration,
      string section = "webSockets:server") {
      return configuration.GetConfiguration<ServerConfiguration>(section);
    }

    public static void AddCorrelationService(this IServeWebSockets service, Guid correlationId) {
      service.Server.AddWebSocketService(correlationId.GetCorrelationEndpoint(leadingForwardSlash: true),
        () => new Correlation(correlationId));
    }

    public static Uri GetCorrelationUri(this IServeWebSockets service, Guid correlationId) {
      return new Uri(service.GetCorrelationUrl(correlationId));
    }

    public static WebSocket CreateCorrelationClient(this IServeWebSockets service, Guid correlationId, bool connect = true) {
      var client = new WebSocket(service.GetCorrelationUrl(correlationId));
      if (connect) client.Connect();
      return client;
    }

    private static string GetCorrelationEndpoint(this Guid correlationId, bool leadingForwardSlash = false) {
      return $"{(leadingForwardSlash ? "/" : "")}correlations/{correlationId}";
    }

    private static string GetCorrelationUrl(this IServeWebSockets service, Guid correlationId) {
      return $"{service.BaseUri}{correlationId.GetCorrelationEndpoint()}";
    }
  }
}
