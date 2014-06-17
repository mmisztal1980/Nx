using MassTransit;
using MassTransit.Reactive;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Nx.Domain.Events
{
    public abstract class DomainEventHandler<TEvent> : Disposable, IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        protected readonly IServiceBus ServiceBus;
        private readonly IDisposable _subscription;

        protected DomainEventHandler(Uri uri)
            : this(uri, Guid.NewGuid())
        {
        }

        protected DomainEventHandler(Uri uri, Guid receiverId)
        {
            ReceiverId = receiverId;

            ServiceBus = ConfigureServiceBus(uri);

            _subscription = ServiceBus.AsObservable<TEvent>()
                .ObserveOn(NewThreadScheduler.Default)
                .Where(ev => ev.ReceiverId.Equals(Guid.Empty) || ev.ReceiverId.Equals(ReceiverId))
                .Subscribe(
                    HandleEvent,
                    HandleException,
                    HandleCompletion);
        }

        public Guid ReceiverId
        {
            get;
            private set;
        }

        public abstract void HandleEvent(TEvent ev);

        protected virtual IServiceBus ConfigureServiceBus(Uri uri)
        {
            return ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(uri);
                bus.UseRabbitMq();
            });
        }

        protected override void Dispose(bool disposing)
        {
        }

        protected void HandleCompletion()
        {
        }

        protected void HandleException<TException>(TException ex) where TException : Exception
        { }
    }
}