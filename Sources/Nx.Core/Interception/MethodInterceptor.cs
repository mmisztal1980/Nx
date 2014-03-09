using Ninject.Extensions.Interception;

namespace Nx.Interception
{
    public abstract class MethodInterceptor : SimpleInterceptor
    {
        private readonly string _methodName;

        protected MethodInterceptor(string methodName)
        {
            _methodName = methodName;
        }

        protected override void BeforeInvoke(IInvocation invocation)
        {
            if (invocation.Request.Method.Name.Equals(_methodName))
            {
                OnInvoke(invocation);
            }
        }

        protected abstract void OnInvoke(IInvocation invocation);
    }
}