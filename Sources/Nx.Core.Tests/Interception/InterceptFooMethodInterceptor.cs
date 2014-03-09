using Ninject.Extensions.Interception;
using Nx.Interception;
using System;

namespace Nx.Core.Tests.Interception
{
    internal class InterceptFooMethodInterceptor : MethodInterceptor
    {
        public const string ResourceName = "FooMethodInterceptor";
        private bool _didRun;

        public InterceptFooMethodInterceptor()
            : base("Foo")
        {
        }

        public bool DidRun
        {
            get
            {
                var result = _didRun;
                _didRun = false;
                return result;
            }
            private set { _didRun = value; }
        }

        protected override void OnInvoke(IInvocation invocation)
        {
            Console.WriteLine("Method intercepted!");
            DidRun = true;
        }
    }
}