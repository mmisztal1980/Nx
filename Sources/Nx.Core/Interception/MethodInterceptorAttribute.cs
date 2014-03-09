using System;

namespace Nx.Interception
{
    /// <summary>
    /// An interceptor that will only intercept calls to a method, matching the methodName
    /// </summary>
    public abstract class MethodInterceptorAttribute : InterceptorAttribute
    {
        protected MethodInterceptorAttribute(Type interceptorType)
            : base(interceptorType)
        {
            if (!typeof(MethodInterceptor).IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException("interceptorType does not inherit MethodInterceptor class",
                    "interceptorType");
            }
        }
    }
}