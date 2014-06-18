using Nx.Events;
using System;

namespace Nx
{
    public interface IAggregate : IEntity
    {
    }

    public interface IAggreate<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
        void Mutate<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    }
}