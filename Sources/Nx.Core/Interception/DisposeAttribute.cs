using System;

namespace Nx.Interception
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.GenericParameter | AttributeTargets.Interface, AllowMultiple = false)]
    public class DisposeAttribute : Attribute
    {
    }
}