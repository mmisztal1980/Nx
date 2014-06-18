using MassTransit;
using System;

namespace Nx.ServiceBus
{
    // ReSharper disable InconsistentNaming
    public static class IServiceBusExtensions
    // ReSharper restore InconsistentNaming
    {
        public static void SendTo<TMessage>(this IServiceBus bus, TMessage message, Guid receiverId)
            where TMessage : class, IServiceBusMessage
        {
            message.ReceiverId = receiverId;
            bus.Publish(message);
        }
    }
}