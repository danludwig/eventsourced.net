using System;
using JetBrains.Annotations;
using SimpleInjector;

namespace EventSourced.Net.Services.Messaging.Queries
{
  [UsedImplicitly]
  internal sealed class QueryExecutor : IExecuteQuery
  {
    private Container Container { get; }

    public QueryExecutor(Container container) {
      Container = container;
    }

    public TResult Execute<TResult>(IQuery<TResult> query) {
      Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(query.GetType(), typeof(TResult));
      dynamic handler = Container.GetInstance(handlerType);
      return handler.Handle((dynamic)query);
    }
  }
}
