using MassTransit;
using MassTransit.Reactive;
using Nx.Extensions;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Nx.Commands
{
    /// <summary>
    /// The ReactiveCommandHandler subscribes to the MassTransit IService bus, listens to incoming commands and handles them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReactiveCommandHandler<T> : Disposable, ICommandHandler<T>
        where T : class, ICommand, CorrelatedBy<Guid>
    {
        protected readonly IServiceBus ServiceBus;
        private readonly IDisposable _subscription;

        protected ReactiveCommandHandler(Uri uri)
            : this(uri, Guid.NewGuid())
        {
        }

        protected ReactiveCommandHandler(Uri uri, Guid receiverId)
        {
            ReceiverId = receiverId;

            ServiceBus = ConfigureServiceBus(uri);

            _subscription = ServiceBus.AsObservable<T>()
                .ObserveOn(NewThreadScheduler.Default)
                .Where(cmd => cmd.ReceiverId.Equals(Guid.Empty) || cmd.ReceiverId.Equals(ReceiverId))
                .Subscribe(
                    HandleCommand,
                    HandleException,
                    HandleCompletion);
        }

        public abstract string Key { get; }

        public Guid ReceiverId { get; private set; }

        public abstract void HandleCommand(T command);

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
            if (disposing)
            {
                this.With(x => x.ServiceBus).Disposal();
                this.With(x => x._subscription).Disposal();
            }
        }

        private void HandleCompletion()
        { }

        private void HandleException<TException>(TException ex)
            where TException : Exception
        {
        }
    }
}