using System;

namespace Nx.Kernel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        public ComponentAttribute(ComponentLifestyle componentLifestyle)
        {
            ComponentLifestyle = componentLifestyle;
        }

        public ComponentLifestyle ComponentLifestyle { get; private set; }
    }
}