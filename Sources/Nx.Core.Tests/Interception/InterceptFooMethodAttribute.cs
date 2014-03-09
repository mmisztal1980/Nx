using Nx.Interception;

namespace Nx.Core.Tests.Interception
{
    internal class InterceptFooMethodAttribute : MethodInterceptorAttribute
    {
        public InterceptFooMethodAttribute()
            : base(typeof(InterceptFooMethodInterceptor))
        { }
    }
}