using System;
using System.Collections.Generic;

//using IInvocation = Ninject.Extensions.Interception.IInvocation;

namespace Nx.Extensions
{
    // ReSharper disable InconsistentNaming
    public static class IEnumerableExtensions
    // ReSharper restore InconsistentNaming
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Condition.ArgumentNotNull(enumerable, "enumerable");
            Condition.ArgumentNotNull(action, "action");

            foreach (var @enum in enumerable)
            {
                action(@enum);
            }
        }

        //internal static void CleanUpIEnumerable(this IEnumerable enumerable, MemberInfo memberInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    foreach (var item in enumerable)
        //    {
        //        var disposable = item as IDisposable;
        //        if (disposable == null) continue;
        //        try
        //        {
        //            disposable.Dispose();
        //            logger.Debug("Item {2} ({3}) from collection {0}.{1} disposed", invocation.Request.Target.GetType().Name, memberInfo.Name, disposable.GetType().Name,
        //                item.GetHashCode());
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Warning("Error while disposing a collection item : {0}", ex.Message);
        //        }
        //    }
        //}

        //internal static void CleanUpIDictionary(this IDictionary dictionary, MemberInfo memberInfo, ref IInvocation invocation, ILogger logger)
        //{
        //    foreach (var value in dictionary.Values)
        //    {
        //        if (!(value is IDisposable)) continue;
        //        try
        //        {
        //            logger.Debug("Item {2} ({3}) from dictionary values {0}.{1} disposed",
        //                invocation.Request.Target.GetType().Name, memberInfo.Name, value.GetType().Name,
        //                value.GetHashCode());
        //            (value as IDisposable).Dispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Warning("Error while disposing a dictionary value : {0}", ex.Message);
        //        }
        //    }

        //    foreach (var key in dictionary.Keys)
        //    {
        //        if (!(key is IDisposable)) continue;
        //        try
        //        {
        //            Console.WriteLine(
        //                string.Format("Item {2} ({3}) from dictionary keys {0}.{1} disposed",
        //                    invocation.Request.Target.GetType().Name, memberInfo.Name, key.GetType().Name,
        //                    key.GetHashCode()), "INFO");
        //            (key as IDisposable).Dispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Warning("Error while disposing a dictionary key : {0}", ex.Message);
        //        }
        //    }

        //    dictionary.Clear();
        //}
    }
}