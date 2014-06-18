using Nx.ServiceBus;
using System;

namespace Nx.Commands
{
    /// <summary>
    /// The basic ICommand containing the ICommand's GUID Id
    /// </summary>
    public interface ICommand : IServiceBusMessage
    {
    }

    public interface ICommand<TEntity, out TId> : ICommand
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        TId EntityId { get; }
    }
}