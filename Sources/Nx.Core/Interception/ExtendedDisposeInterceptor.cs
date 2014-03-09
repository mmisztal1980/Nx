using Ninject.Extensions.Interception;
using Nx.Extensions;
using Nx.Logging;

namespace Nx.Interception
{
    /// <summary>
    /// Intercepts IDisposable.Dispose calls.
    /// (!) The Dispose method must be vitual
    /// </summary>
    public sealed class ExtendedDisposeInterceptor : MethodInterceptor
    {
        private readonly ILogger _logger;

        public ExtendedDisposeInterceptor(ILogFactory logFactory)
            : base("Dispose")
        {
            _logger = logFactory.CreateLogger("SYSTEM");
        }

        protected override void OnInvoke(IInvocation invocation)
        {
            var targetType = invocation.Request.Target.GetType();

            targetType.GetFields<DisposeAttribute>().ForEach(fi => fi.DisposeFieldInfo(ref invocation, _logger));
            targetType.GetProperties<DisposeAttribute>().ForEach(pi => pi.DisposePropertyInfo(ref invocation, _logger));
        }
    }
}