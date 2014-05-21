using System;

namespace Nx.Domain.Commands
{
    /// <summary>
    /// The basic ICommand containing the ICommand's GUID Id
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The Command's Id. The implementing command should set the Id's value in the c-tor.
        /// The ICommandHandler will not handle a command with a missing Id
        /// </summary>
        Guid Id { get; }
    }

    public interface ICommand<in T> : ICommand
        where T : class
    {
    }
}