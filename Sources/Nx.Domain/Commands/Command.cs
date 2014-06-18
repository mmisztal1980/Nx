using Nx.ServiceBus;
using System;

namespace Nx.Commands
{
    /// <summary>
    /// An abstract Command DTO, implementing ICommand.
    /// Always has an initialized Id.
    /// Serializable
    /// </summary>
    [Serializable]
    public abstract class Command : ServiceBusMessage, ICommand
    {
        /// <summary>
        /// Creates a new Command instance and sets the Id value to a new Guid.
        /// </summary>
        protected Command()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Creates a new Command instance and sets the Id value to passed value.
        /// </summary>
        /// <param name="id"></param>
        protected Command(Guid id)
            : base(id)
        {
            Condition.Require<InvalidOperationException>(!id.Equals(Guid.Empty), "You cannot use an empty GUID for a command Id");
            Id = id;
        }
    }
}