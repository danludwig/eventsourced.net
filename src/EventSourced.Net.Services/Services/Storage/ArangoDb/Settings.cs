using System;

namespace EventSourced.Net.Services.Storage.ArangoDb
{
  // todo: make this configurable from json
  public class Settings
  {
    public Uri ServerUri { get; } = new Uri("http://localhost:8529"); // mac try http://localhost:8000
    public string ServerUrl => ServerUri?.ToString();
    public string DbName { get; } = "EventSourced";
  }
}
