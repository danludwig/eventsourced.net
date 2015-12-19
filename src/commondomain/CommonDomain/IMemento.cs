using System;

namespace CommonDomain
{
  public interface IMemento
  {
    Guid Id { get; set; }
    int Version { get; set; }
  }
}