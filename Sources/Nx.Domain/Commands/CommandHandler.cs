using System;

namespace Nx.Domain.Commands
{
    public abstract class CommandHandler<T> : ICommandHandler<T>
        where T : class, ICommand
    {
        private readonly IDisposable _subscription;

        protected CommandHandler(MassTransit.IServiceBus bus, string queueName)
        {
        }

        public abstract void HandleCommand(T command);
    }
}