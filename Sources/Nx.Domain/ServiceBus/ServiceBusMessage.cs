using System;

namespace Nx.ServiceBus
{
    /// <summary>
    /// ServiceBusMessage is the most base type of messages passed by the ServiceBus
    /// </summary>
    [Serializable]
    public class ServiceBusMessage : IServiceBusMessage
    {
        private ServiceBusMessage()
        {
            ReceiverId = Guid.Empty;
        }

        protected ServiceBusMessage(Guid id)
        {
            Condition.Require<InvalidOperationException>(!id.Equals(Guid.Empty), "You cannot use an empty GUID for a ServiceBusMessage Id");
            Id = id;
        }

        /// <summary>
        /// CorellationId for MassTransit
        /// </summary>
        public Guid CorrelationId { get { return Id; } }

        /// <summary>
        /// Id of the message
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ReceiverId - if different from Guid.Empty, a Receiver with matching Id will be expected to process that message.
        /// Initially empty. Expected to be set prior to publishing
        /// </summary>
        public Guid ReceiverId { get; set; }
    }
}