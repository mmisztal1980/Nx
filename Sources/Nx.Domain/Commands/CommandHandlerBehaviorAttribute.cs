using System;

namespace Nx.Commands
{
    /// <summary>
    /// The enum used to describe the command handler's behavior
    /// </summary>
    public enum CommandHandlerBehavior
    {
        Competing,      // Only a single handler will handle the command
        NonCompeting    // All subscribing handlers will handle the command
    }

    /// <summary>
    /// CommandHandlerBehaviorAttribute is an attribute required by implementations All classes deriving from CommandHandler.
    /// The attribute describes the CommandHandler's behavior in a distributed environment
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CommandHandlerBehaviorAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="relativeUri">example : /myCommandHandler</param>
        /// <param name="behavior"></param>
        public CommandHandlerBehaviorAttribute(string relativeUri, CommandHandlerBehavior behavior)
        {
            RelativeUri = relativeUri;
            Behavior = behavior;
        }

        /// <summary>
        /// The Behavior. If set to competing the RelativeUri will be used for
        /// all instances of the ICommandHandler implementation, making them compete.
        /// If set to NonCompeting a guid will be attached at the end of the RelativeUri
        /// </summary>
        public CommandHandlerBehavior Behavior { get; private set; }

        public string RelativeUri { get; private set; }
    }
}