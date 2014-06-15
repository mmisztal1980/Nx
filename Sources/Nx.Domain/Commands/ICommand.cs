using Nx.Domain.ServiceBus;

namespace Nx.Domain.Commands
{
    /// <summary>
    /// The basic ICommand containing the ICommand's GUID Id
    /// </summary>
    public interface ICommand : IServiceBusMessage
    {
    }

    public interface ICommand<in T> : ICommand
        where T : class
    {
    }
}