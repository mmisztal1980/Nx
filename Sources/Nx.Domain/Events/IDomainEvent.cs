using Nx.ServiceBus;

namespace Nx.Events
{
    public interface IDomainEvent : IServiceBusMessage
    {
    }
}