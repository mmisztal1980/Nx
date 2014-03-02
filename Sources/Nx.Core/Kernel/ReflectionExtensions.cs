using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nx.Kernel
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypesFromAssemblyWithMatchingAttribute<TAttribute>(Assembly assembly)
            where TAttribute : Attribute
        {
            var types = assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(TAttribute)));
            return types;
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector)
        {
            return default(TValue);
        }
    }
}