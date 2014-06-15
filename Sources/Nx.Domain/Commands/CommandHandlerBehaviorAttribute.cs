using System;

namespace Nx.Domain.Commands
{
    /// <summary>
    /// CommandHandlerBehaviorAttribute is an attribute required by implementations All classes deriving from CommandHandler.
    /// The attribute describes the CommandHandler's behavior in a distributed environment
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CommandHandlerBehaviorAttribute : Attribute
    {
    }

    /// <summary>
    /// The enum used to describe the command handler's behavior
    /// </summary>
    public enum CommandHandlerBehavior
    {
        Competing,      // Only a single handler will handle the command
        NonCompeting    // All subscribing handlers will handle the command
    }
}