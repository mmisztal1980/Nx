using System;

namespace Nx.Core.Tests.Interception
{
    public class FooMethodInterceptionTestObject
    {
        [InterceptFooMethod]
        public virtual void Foo()
        {
            Console.WriteLine("Foo");
        }

        [InterceptFooMethod]
        public virtual void Bar()
        {
            Console.WriteLine("Bar");
        }
    }
}