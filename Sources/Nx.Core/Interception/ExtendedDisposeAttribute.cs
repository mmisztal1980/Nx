using System;

namespace Nx.Interception
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ExtendedDisposeAttribute : MethodInterceptorAttribute
    {
        public ExtendedDisposeAttribute()
            : base(typeof(ExtendedDisposeInterceptor))
        {
        }
    }
}