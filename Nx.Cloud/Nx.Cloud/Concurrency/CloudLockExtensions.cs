using System;

namespace Nx.Cloud.Concurrency
{
    public static class CloudLockExtensions
    {
        /// <summary>
        /// Executes the action if the @lock is aquired and disposes the @lock to release the lease
        /// </summary>
        /// <param name="lock">The named lock</param>
        /// <param name="action">The action to execute</param>
        public static void Execute(this CloudLock @lock, Action action)
        {
            try
            {
                if (@lock.HasLock)
                {
                    action();
                }

            }
            finally
            {
                @lock.Dispose();
            }
        }

        /// <summary>
        /// Executes the action if the @lock is aquired and disposes the @lock to release the lease
        /// </summary>
        /// <param name="lock">The named lock</param>
        /// <param name="action">The action to execute</param>
        /// <param name="param">The action parameter</param>
        public static void Execute<T>(this CloudLock @lock, Action<T> action, T param)
        {
            try
            {
                if (@lock.HasLock)
                {
                    action(param);
                }
            }
            finally
            {
                @lock.Dispose();
            }
        }
    }
}
