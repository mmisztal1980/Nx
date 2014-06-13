using MassTransit;
using MassTransit.Reactive;
using Nx.Extensions;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Nx.Domain.Commands
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
        {
            ServiceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(uri);
                bus.UseRabbitMq();
            });

            _subscription = ServiceBus.AsObservable<T>()
                .ObserveOn(NewThreadScheduler.Default)
                .Subscribe(
                    HandleCommand,
                    HandleException,
                    HandleCompletion);
        }

        public abstract void HandleCommand(T command);

        public abstract string Key { get; }

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
        { }
    }
}