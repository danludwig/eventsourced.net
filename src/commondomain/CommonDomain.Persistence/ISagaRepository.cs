using System;
using System.Collections.Generic;

namespace CommonDomain.Persistence
{
  public interface ISagaRepository
  {
    TSaga GetById<TSaga>(Guid sagaId) where TSaga : class, ISaga, new();
    void Save(ISaga saga, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
  }
}