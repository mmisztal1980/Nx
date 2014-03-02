using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nx.Kernel
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets a value from an attribute using a valueSelector delegate
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="type"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var attribute = type.GetCustomAttributes(typeof(TAttribute), true)
                .FirstOrDefault() as TAttribute;

            if (attribute != null)
            {
                return valueSelector(attribute);
            }
            return default(TValue);
        }

        /// <summary>
        /// Gets an attribute from a type
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static TAttribute GetAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            var attribute = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attribute;
        }

        /// <summary>
        /// Retrieves a list of types from an assembly with a matching attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> GetTypesWithAttributeFromAssembly<TAttribute>(Assembly assembly)
            where TAttribute : Attribute
        {
            return assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(TAttribute)));
        }
    }
}