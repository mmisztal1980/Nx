using System;

namespace Nx.ServiceBus
{
    public interface IServiceBusReceiver
    {
        Guid ReceiverId { get; }
    }
}