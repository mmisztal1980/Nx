﻿using Ninject;
using Nx.Logging;
using System;
using System.Linq;

namespace Nx.Kernel
{
    public static class KernelExtensions
    {
        #region IsRegistered

        public static bool IsRegistered<T>(this IKernel kernel)
        {
            return kernel.IsRegistered(typeof(T));
        }

        public static bool IsRegistered(this IKernel kernel, Type type)
        {
            return kernel.GetBindings(type).Any();
        }

        #endregion IsRegistered

        #region RegisterInstance

        public static void RegisterInstance<T>(this IKernel kernel, T instance)
        {
            using (var log = kernel.GetLogger("SYSTEM"))
            {
                log.Info("Registering instance of type {0}", instance.GetType().Name);
                kernel.Bind<T>().ToConstant(instance);
            }
        }

        public static void RegisterInstance<TFrom, TTo>(this IKernel kernel, TTo instance)
            where TTo : TFrom
        {
            using (var log = kernel.GetLogger("SYSTEM"))
            {
                log.Info("Registering instance of type {0}", typeof(TFrom).Name);
                kernel.Bind<TFrom>().ToConstant(instance);
            }
        }

        #endregion RegisterInstance

        #region RegisterTypeIfMissing<TType>

        public static void RegisterTypeIfMissing<TType>(this IKernel kernel, bool singleton)
        {
            kernel.RegisterTypeIfMissing(typeof(TType), singleton);
        }

        public static void RegisterTypeIfMissing(this IKernel kernel, Type type, bool singleton)
        {
            using (var log = kernel.GetLogger("SYSTEM"))
            {
                if (!kernel.IsRegistered(type))
                {
                    if (singleton)
                    {
                        kernel.Bind(type).ToSelf().InSingletonScope();
                    }
                    else
                    {
                        kernel.Bind(type).ToSelf();
                    }
                }
                log.Debug("Type registration : {0} -> Self ({1})", type.Name, singleton);
            }
        }

        public static void RegisterTypeIfMissing<TType>(this IKernel kernel, string key, bool singleton)
        {
            kernel.RegisterTypeIfMissing(typeof(TType), key, singleton);
        }

        public static void RegisterTypeIfMissing(this IKernel kernel, Type type, string key, bool singleton)
        {
            using (var log = kernel.GetLogger("SYSTEM"))
            {
                if (!kernel.IsRegistered(type))
                {
                    if (singleton)
                    {
                        kernel.Bind(type).ToSelf().InSingletonScope().Named(key);
                    }
                    else
                    {
                        kernel.Bind(type).ToSelf().Named(key);
                    }
                }
                log.Debug("Type registration : {0} -> Self ({1})", type.Name, singleton);
            }
        }

        #endregion RegisterTypeIfMissing<TType>

        #region RegisterTypeIfMissing<TFrom TTo>

        public static void RegisterTypeIfMissing<TFrom, TTo>(this IKernel kernel, bool singleton)
        {
            kernel.RegisterTypeIfMissing(typeof(TFrom), typeof(TTo), singleton);
        }

        public static void RegisterTypeIfMissing<TFrom, TTo>(this IKernel kernel, string key, bool singleton)
        {
            kernel.RegisterTypeIfMissing(typeof(TFrom), typeof(TTo), key, singleton);
        }

        public static void RegisterTypeIfMissing(this IKernel kernel, Type from, Type to, bool singleton)
        {
            using (var log = kernel.GetLogger("SYSTEM"))
            {
                if (!kernel.IsRegistered(from))
                {
                    if (singleton)
                    {
                        kernel.Bind(from).To(to).InSingletonScope();
                    }
                    else
                    {
                        kernel.Bind(from).To(to);
                    }
                }
                log.Debug("Type registration : {0} -> {1} ({2})", from.Name, to.Name, singleton);
            }
        }

        public static void RegisterTypeIfMissing(this IKernel kernel, Type from, Type to, string key, bool singleton)
        {
            using (var log = kernel.GetLogger("SYSTEM"))
            {
                if (!kernel.IsRegistered(from))
                {
                    if (singleton)
                    {
                        kernel.Bind(from).To(to).InSingletonScope().Named(key);
                    }
                    else
                    {
                        kernel.Bind(from).To(to).Named(key);
                    }
                }
                log.Debug("Type registration : {0} -> {1} ({2}) [{3}]", from.Name, to.Name, singleton, key);
            }
        }

        #endregion RegisterTypeIfMissing<TFrom TTo>

        #region UnregisterType<TType>

        public static void UnregisterType<TType>(this IKernel kernel)
        {
            if (kernel.IsRegistered<TType>())
            {
                kernel.Release(kernel.Get<TType>());
                kernel.Unbind<TType>();
            }
        }

        #endregion UnregisterType<TType>

        internal static ILogger GetLogger(this IKernel kernel, string loggerName)
        {
            return kernel.Get<ILogFactory>().CreateLogger(loggerName);
        }
    }
}