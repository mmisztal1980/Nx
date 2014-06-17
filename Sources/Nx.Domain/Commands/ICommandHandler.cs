using Nx.Domain.ServiceBus;
using System;

namespace Nx.Domain.Commands
{
    public interface ICommandHandler<in T> : IServiceBusReceiver, IDisposable
        where T : class, ICommand
    {
        /// <summary>
        /// Handles the command in a safe way. Exception handling is done inside. Does not throw.
        /// </summary>
        /// <param name="command">The command to handle</param>
        void HandleCommand(T command);
    }
}