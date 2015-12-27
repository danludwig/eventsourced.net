using System;
using JetBrains.Annotations;
using SimpleInjector;

namespace EventSourced.Net.Services.Messaging.Queries
{
  [UsedImplicitly]
  internal sealed class QueryProcessor : IProcessQuery
  {
    private Container Container { get; }

    public QueryProcessor(Container container) {
      Container = container;
    }

    public TResult Execute<TResult>(IQuery<TResult> query) {
      Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(query.GetType(), typeof(TResult));
      dynamic handler = Container.GetInstance(handlerType);
      return handler.Handle((dynamic)query);
    }
  }
}
