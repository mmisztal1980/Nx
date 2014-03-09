using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IInvocation = Ninject.Extensions.Interception.IInvocation;
using ILogger = Nx.Logging.ILogger;

namespace Nx.Extensions
{
    public static class ReflectionExtensions
    {
        public static void DisposeFieldInfo(this FieldInfo fieldInfo, IInvocation invocation, ILogger logger)
        {
            Console.WriteLine("Cleaning up {0}", fieldInfo.Name);

            if (!fieldInfo.CleanUpIEnumerable(invocation, logger))
                if (!fieldInfo.CleanUpIDictionary(invocation, logger))
                    fieldInfo.CleanUpFieldInfo(invocation);

            if (fieldInfo.FieldType.IsClass || fieldInfo.FieldType.IsInterface)
            {
                fieldInfo.SetValue(invocation.Request.Target, null);
            }
        }

        public static void DisposePropertyInfo(this PropertyInfo propertyInfo, IInvocation invocation, ILogger logger)
        {
            Console.WriteLine("Cleaning up {0}", propertyInfo.Name);

            if (!propertyInfo.CleanUpIEnumerable(invocation, logger))
                if (!propertyInfo.CleanUpIDictionary(invocation, logger))
                    propertyInfo.CleanUpPropertyInfo(invocation);

            if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
            {
                propertyInfo.SetValue(invocation.Request.Target, null);
            }
        }

        /// <summary>
        /// Returns all fields, from a given type, flagged with a [TAttribute]
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetFields<TAttribute>(this Type type)
                                    where TAttribute : Attribute
        {
            var result = type.GetAllFields().Where(field => Attribute.IsDefined(field, typeof(TAttribute)));
            return result;
        }

        /// <summary>
        /// Returns all properties, from a given type, flagged with a [TAttribute]
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            var result = type.GetAllProperties().Where(property => Attribute.IsDefined(property, typeof(TAttribute)));
            return result;
        }

        private static void CleanUpFieldInfo(this FieldInfo fieldInfo, IInvocation invocation)
        {
            var obj = fieldInfo.GetValue(invocation.Request.Target) as IDisposable;
            if (obj != null)
            {
                obj.Dispose();
            }
        }

        private static bool CleanUpIDictionary(this FieldInfo fieldInfo, IInvocation invocation, ILogger logger)
        {
            var dictionary = fieldInfo.GetValue(invocation.Request.Target) as System.Collections.IDictionary;
            if (dictionary == null) return false;

            dictionary.CleanUpIDictionary(fieldInfo, invocation, logger);

            return true;
        }

        private static bool CleanUpIDictionary(this PropertyInfo propertyInfo, IInvocation invocation, ILogger logger)
        {
            var dictionary = propertyInfo.GetValue(invocation.Request.Target) as System.Collections.IDictionary;
            if (dictionary == null) return false;

            dictionary.CleanUpIDictionary(propertyInfo, invocation, logger);

            return true;
        }

        private static bool CleanUpIEnumerable(this FieldInfo fieldInfo, IInvocation invocation, ILogger logger)
        {
            var collection = fieldInfo.GetValue(invocation.Request.Target) as System.Collections.IEnumerable;
            if (collection == null) return false;

            collection.CleanUpIEnumerable(fieldInfo, invocation, logger);

            return true;
        }

        private static bool CleanUpIEnumerable(this PropertyInfo propertyInfo, IInvocation invocation, ILogger logger)
        {
            var collection = propertyInfo.GetValue(invocation.Request.Target) as System.Collections.IEnumerable;
            if (collection == null) return false;

            collection.CleanUpIEnumerable(propertyInfo, invocation, logger);

            return true;
        }

        private static void CleanUpPropertyInfo(this PropertyInfo propertyInfo, IInvocation invocation)
        {
            Console.WriteLine("Cleaning up {0}", propertyInfo.Name);
            var obj = propertyInfo.GetValue(invocation.Request.Target) as IDisposable;
            if (obj != null)
            {
                obj.Dispose();
            }

            if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
            {
                propertyInfo.SetValue(invocation.Request.Target, null);
            }
        }

        /// <summary>
        /// Returns all fields from a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<FieldInfo> GetAllFields(this Type type)
        {
            var result = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance |
                               BindingFlags.Public | BindingFlags.FlattenHierarchy);
            return result;
        }

        /// <summary>
        /// Returns all properties from a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            var result = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return result;
        }
    }
}