using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

//using IInvocation = Ninject.Extensions.Interception.IInvocation;

namespace Nx.Extensions
{
    public static class ReflectionExtensions
    {
        //public static void DisposeFieldInfo(this FieldInfo fieldInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    Console.WriteLine("Cleaning up {0}", fieldInfo.Name);

        //    if (!fieldInfo.CleanUpIEnumerable(ref invocation, logger))
        //        if (!fieldInfo.CleanUpIDictionary(ref invocation, logger))
        //            fieldInfo.CleanUpFieldInfo(ref invocation);

        //    if (fieldInfo.FieldType.IsClass || fieldInfo.FieldType.IsInterface)
        //    {
        //        fieldInfo.SetValue(invocation.Request.Target, null);
        //    }
        //}

        //public static void DisposePropertyInfo(this PropertyInfo propertyInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    Console.WriteLine("Cleaning up {0}", propertyInfo.Name);

        //    if (!propertyInfo.CleanUpIEnumerable(ref invocation, logger))
        //        if (!propertyInfo.CleanUpIDictionary(ref invocation, logger))
        //            propertyInfo.CleanUpPropertyInfo(ref invocation);

        //    if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
        //    {
        //        propertyInfo.SetValue(invocation.Request.Target, null);
        //    }
        //}

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

        //private static void CleanUpFieldInfo(this FieldInfo fieldInfo, ref IInvocation invocation)
        //{
        //    var obj = fieldInfo.GetValue(invocation.Request.Target) as IDisposable;
        //    if (obj != null)
        //    {
        //        obj.Dispose();
        //    }
        //}

        //private static bool CleanUpIDictionary(this FieldInfo fieldInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    var dictionary = fieldInfo.GetValue(invocation.Request.Target) as System.Collections.IDictionary;
        //    if (dictionary == null) return false;

        //    dictionary.CleanUpIDictionary(fieldInfo, ref invocation, logger);

        //    return true;
        //}

        //private static bool CleanUpIDictionary(this PropertyInfo propertyInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    var dictionary = propertyInfo.GetValue(invocation.Request.Target) as System.Collections.IDictionary;
        //    if (dictionary == null) return false;

        //    dictionary.CleanUpIDictionary(propertyInfo, ref invocation, logger);

        //    return true;
        //}

        //private static bool CleanUpIEnumerable(this FieldInfo fieldInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    var collection = fieldInfo.GetValue(invocation.Request.Target) as System.Collections.IEnumerable;
        //    if (collection == null) return false;

        //    collection.CleanUpIEnumerable(fieldInfo, ref invocation, logger);

        //    return true;
        //}

        //private static bool CleanUpIEnumerable(this PropertyInfo propertyInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    var collection = propertyInfo.GetValue(invocation.Request.Target) as System.Collections.IEnumerable;
        //    if (collection == null) return false;

        //    collection.CleanUpIEnumerable(propertyInfo, ref invocation, logger);

        //    return true;
        //}

        //private static void CleanUpPropertyInfo(this PropertyInfo propertyInfo, ref IInvocation invocation)
        //{
        //    Console.WriteLine("Cleaning up {0}", propertyInfo.Name);
        //    var obj = propertyInfo.GetValue(invocation.Request.Target) as IDisposable;
        //    if (obj != null)
        //    {
        //        obj.Dispose();
        //    }
        //}

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