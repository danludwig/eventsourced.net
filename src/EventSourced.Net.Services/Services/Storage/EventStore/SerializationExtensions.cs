using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventSourced.Net.Services.Storage.EventStore
{
  public static class SerializationExtensions
  {
    private const string EventClrTypeHeader = "EventClrTypeName";
    private static readonly JsonSerializerSettings SerializerSettings;

    static SerializationExtensions() {
      SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
    }

    #region Serialization

    public static EventData ToEventData(this object data, IDictionary<string, object> metadata, Guid? eventId = null) {
      eventId = eventId ?? Guid.NewGuid();
      byte[] dataBytes = data.Serialize();
      byte[] metadatBytes = metadata.Serialize(data);
      string typeName = data.GetType().FullName;
      var eventData = new EventData(eventId.Value, typeName, true, dataBytes, metadatBytes);
      return eventData;
    }

    private static byte[] Serialize(this object data) {
      return data.ToByteArray();
    }

    private static byte[] Serialize(this IDictionary<string, object> metadata, object data) {
      IDictionary<string, object> eventHeaders = new Dictionary<string, object>(metadata) {
        { EventClrTypeHeader, $"{data.GetType().FullName}, {data.GetType().Assembly.GetName().Name}" },
      };
      return eventHeaders.ToByteArray();
    }

    private static byte[] ToByteArray(this object data) {
      return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, SerializerSettings));
    }

    #endregion
    #region Deserialization

    public static object ToEventObject(this RecordedEvent recordedEvent) {
      return Deserialize(recordedEvent.Data, recordedEvent.Metadata);
    }

    internal static object Deserialize(byte[] data, byte[] metadata) {
      string metadataString = metadata.ToJson();
      JToken eventClrTypeNameToken = JObject.Parse(metadataString)
        .Property(EventClrTypeHeader)
        .Value;
      string eventClrTypeName = (string)eventClrTypeNameToken;

      string eventDataString = data.ToJson();
      Type eventType = Type.GetType(eventClrTypeName);
      object eventObject = JsonConvert.DeserializeObject(eventDataString, eventType);
      return eventObject;
    }

    private static string ToJson(this byte[] data) {
      return Encoding.UTF8.GetString(data);
    }

    #endregion
  }
}
