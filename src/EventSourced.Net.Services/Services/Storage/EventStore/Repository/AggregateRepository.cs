using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonDomain.Persistence;
using EventStore.ClientAPI;

namespace EventSourced.Net.Services.Storage.EventStore.Repository
{
  public class AggregateRepository : IRepository
  {
    private const string AggregateClrTypeHeader = "AggregateClrTypeName";
    private const string CommitIdHeader = "CommitId";
    private const int WritePageSize = 500;
    private const int ReadPageSize = 500;
    private readonly Func<Type, Guid, string> _aggregateIdToStreamName =
      (type, guid) => $"{char.ToLower(type.Name[0]) + type.Name.Substring(1)}-{guid.ToString("N")}";

    private readonly IProvideConnection _eventStoreConnectionProvider;

    public AggregateRepository(IProvideConnection eventStoreConnectionProvider) {
      _eventStoreConnectionProvider = eventStoreConnectionProvider;
    }

    public Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : class, CommonDomain.IAggregate {
      return GetByIdAsync<TAggregate>(id, int.MaxValue);
    }

    public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id, int version) where TAggregate : class, CommonDomain.IAggregate {
      if (version <= 0)
        throw new InvalidOperationException("Cannot get version <= 0");

      string streamName = _aggregateIdToStreamName(typeof(TAggregate), id);
      TAggregate aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);

      int sliceStart = 0;
      StreamEventsSlice currentSlice;
      IEventStoreConnection eventStoreConnection = await _eventStoreConnectionProvider.GetConnectionAsync();
      do {
        int sliceCount = sliceStart + ReadPageSize <= version
                            ? ReadPageSize
                            : version - sliceStart + 1;

        currentSlice = eventStoreConnection.ReadStreamEventsForwardAsync(streamName, sliceStart, sliceCount, false).Result;

        if (currentSlice.Status == SliceReadStatus.StreamNotFound)
          throw new AggregateNotFoundException(id, typeof(TAggregate));

        if (currentSlice.Status == SliceReadStatus.StreamDeleted)
          throw new AggregateDeletedException(id, typeof(TAggregate));

        sliceStart = currentSlice.NextEventNumber;

        foreach (ResolvedEvent evnt in currentSlice.Events)
          aggregate.ApplyEvent(evnt.OriginalEvent.ToEventObject());
      } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

      if (aggregate.Version != version && version < int.MaxValue)
        throw new AggregateVersionException(id, typeof(TAggregate), aggregate.Version, version);

      return aggregate;
    }

    public async Task SaveAsync(CommonDomain.IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateMetadata) {
      var commitMetadata = new Dictionary<string, object>
      {
          {CommitIdHeader, commitId},
          {AggregateClrTypeHeader, aggregate.GetType().FullName}
      };
      updateMetadata?.Invoke(commitMetadata);

      var streamName = _aggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
      var newEvents = aggregate.GetUncommittedEvents().Cast<object>().ToList();
      var originalVersion = aggregate.Version - newEvents.Count;
      var expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion - 1;
      var eventsToSave = newEvents.Select(e => e.ToEventData(commitMetadata)).ToList();

      IEventStoreConnection eventStoreConnection = await _eventStoreConnectionProvider.GetConnectionAsync();
      if (eventsToSave.Count < WritePageSize) {
        await eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave);
      } else {
        var transaction = await eventStoreConnection.StartTransactionAsync(streamName, expectedVersion);

        var position = 0;
        while (position < eventsToSave.Count) {
          var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
          await transaction.WriteAsync(pageEvents);
          position += WritePageSize;
        }

        await transaction.CommitAsync();
      }

      aggregate.ClearUncommittedEvents();
    }
  }
}
