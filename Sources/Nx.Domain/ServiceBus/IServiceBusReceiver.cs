using System;

namespace Nx.Domain.ServiceBus
{
    public interface IServiceBusReceiver
    {
        Guid ReceiverId { get; }
    }
}