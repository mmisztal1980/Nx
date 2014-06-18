using Nx.ServiceBus;
using System;

namespace Nx.Events
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IDomainEventHandler<in TEvent> : IServiceBusReceiver, IDisposable
        where TEvent : class, IDomainEvent
    {
        void HandleEvent(TEvent ev);
    }
}