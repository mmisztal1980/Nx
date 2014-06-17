using Nx.Domain.ServiceBus;
using System;

namespace Nx.Domain.Events
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IDomainEventHandler<TEvent> : IServiceBusReceiver, IDisposable
        where TEvent : class, IDomainEvent
    {
        void HandleEvent(TEvent ev);
    }
}