using System;

namespace Nx
{
    /// <summary>
    /// Very light version of Code Contracts
    /// </summary>
    public static class Condition
    {
        /// <summary>
        /// Will check the condition and throw if the check fails
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="predicate"></param>
        public static void Require<TException>(bool predicate)
            where TException : Exception
        {
            Require<TException>(predicate, string.Empty);
        }

        /// <summary>
        /// Will check the condition and throw if the check fails
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="predicate"></param>
        public static void Require<TException>(bool predicate, string message)
            where TException : Exception
        {
            if (!predicate)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw (TException)Activator.CreateInstance(typeof(TException));
                }
                else
                {
                    throw (TException)Activator.CreateInstance(typeof(TException), message);
                }
            }
        }

        public static void ArgumentNotNull<T>(T argument, string name)
        {
            Require<ArgumentNullException>(argument != null, name);
        }

        public static void ArgumentNotNullOrEmpty(string argument, string name)
        {
            Require<ArgumentNullException>(!string.IsNullOrEmpty(argument), name);
        }
    }
}
