using Nx.Domain.ServiceBus;

namespace Nx.Domain.Events
{
    public interface IDomainEvent : IServiceBusMessage
    {
    }
}