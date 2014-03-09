using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using Ninject.Parameters;
using System;
using System.Globalization;

namespace Nx.Interception
{
    /// <summary>
    /// The base interceptor attribute. Provides actual interceptor resolution
    /// </summary>
    public abstract class InterceptorAttribute : InterceptAttribute
    {
        private readonly Type _interceptorType;
        private readonly IParameter[] _constructorParameters;

        protected InterceptorAttribute(Type interceptorType, params IParameter[] constructorParameters)
        {
            Condition.Require<ArgumentException>(typeof(IInterceptor).IsAssignableFrom(interceptorType),
                string.Format(CultureInfo.InvariantCulture, "{0} does not implement IInterceptor interface", interceptorType.FullName));

            _interceptorType = interceptorType;
            _constructorParameters = constructorParameters;
        }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return (IInterceptor)request.Kernel.Get(_interceptorType, _constructorParameters);
        }
    }
}