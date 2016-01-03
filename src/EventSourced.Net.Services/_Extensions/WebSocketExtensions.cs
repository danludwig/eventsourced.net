using System;
using System.Linq;
using EventSourced.Net.Services.Web.Sockets;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebSocketSharp;

namespace EventSourced.Net
{
  public static class WebSocketExtensions
  {
    public static ServerConfiguration GetWebSocketServerConfiguration(this IConfiguration configuration,
      string section = "webSockets:server") {
      return configuration.GetConfiguration<ServerConfiguration>(section);
    }

    public static void AddCorrelationService(this IServeWebSockets service, ShortGuid correlationId) {
      service.Server.AddWebSocketService(correlationId.GetCorrelationEndpoint(leadingForwardSlash: true),
        () => new Correlation(correlationId));
    }

    public static void RemoveCorrelationService(this IServeWebSockets service, ShortGuid correlationId) {
      service.Server.RemoveWebSocketService(correlationId.GetCorrelationEndpoint(leadingForwardSlash: true));
    }

    public static Uri GetCorrelationUri(this IServeWebSockets service, ShortGuid correlationId) {
      return new Uri(service.GetCorrelationUrl(correlationId));
    }

    public static bool IsCorrelationService(this IServeWebSockets service, ShortGuid correlationId) {
      var endpoint = correlationId.GetCorrelationEndpoint(leadingForwardSlash: true);
      var isService = service.Server.WebSocketServices.Paths.Any(x => string.Equals(x, endpoint, StringComparison.OrdinalIgnoreCase));
      return isService;
    }

    public static WebSocket CreateCorrelationClient(this IServeWebSockets service, ShortGuid correlationId, bool connect = true) {
      var client = new WebSocket(service.GetCorrelationUrl(correlationId));
      if (connect) client.Connect();
      return client;
    }

    private static string GetCorrelationEndpoint(this ShortGuid correlationId, bool leadingForwardSlash = false) {
      return $"{(leadingForwardSlash ? "/" : "")}correlations/{correlationId}";
    }

    private static string GetCorrelationUrl(this IServeWebSockets service, ShortGuid correlationId) {
      return $"{service.BaseUri}{correlationId.GetCorrelationEndpoint()}";
    }

    private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings {
      ContractResolver = new CamelCasePropertyNamesContractResolver(),
    };

    public static void Send(this WebSocket webSocket, object message) {
      var messageJson = JsonConvert.SerializeObject(message, JsonSerializerSettings);
      webSocket.Send(messageJson);
    }
  }
}
