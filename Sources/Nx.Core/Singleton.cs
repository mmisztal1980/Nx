
namespace Nx
{
    /// <summary>
    /// Generic singleton implementation
    /// </summary>
    /// <typeparam name="T">Any class with a default constructor</typeparam>
    public abstract class Singleton<T>
        where T : class, new()
    {
        private static object _lock = new object();

        /// <summary>
        /// The Singleton instance
        /// </summary>
        private static T _Instance;
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new T();
                    }
                }

                return _Instance;
            }
        }
    }
}
