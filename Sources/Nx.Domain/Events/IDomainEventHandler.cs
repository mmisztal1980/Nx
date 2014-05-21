namespace Nx.Domain.Events
{
    public interface IDomainEventHandler<TEvent> where TEvent : class, IDomainEvent
    {
    }
}