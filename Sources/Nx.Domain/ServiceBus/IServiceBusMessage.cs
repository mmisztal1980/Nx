using MassTransit;
using System;

namespace Nx.ServiceBus
{
    public interface IServiceBusMessage : CorrelatedBy<Guid>
    {
        /// <summary>
        /// The Message's Id. The implementing message should set the Id's value in the c-tor.
        /// Any message handler should not a message with a missing Id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// The Id of the intended receiver.
        /// </summary>
        Guid ReceiverId { get; set; }
    }
}