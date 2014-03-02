using Ninject;
using System;
using System.Collections.Generic;

namespace Nx.Web
{
    public class UniversalDependencyResolver :
        Microsoft.AspNet.SignalR.DefaultDependencyResolver,
        System.Web.Http.Dependencies.IDependencyResolver,
        System.Web.Mvc.IDependencyResolver,
        Microsoft.AspNet.SignalR.IDependencyResolver
    {
        private readonly IKernel Kernel;

        public UniversalDependencyResolver(IKernel kernel)
        {
            Kernel = kernel;
        }

        #region WebAPI Controller Resolver
        System.Web.Http.Dependencies.IDependencyScope System.Web.Http.Dependencies.IDependencyResolver.BeginScope()
        {
            return this;
        }

        object System.Web.Http.Dependencies.IDependencyScope.GetService(Type serviceType)
        {
            return GetServiceInternal(serviceType);
        }

        IEnumerable<object> System.Web.Http.Dependencies.IDependencyScope.GetServices(Type serviceType)
        {
            return GetServicesInternal(serviceType);
        }

        void IDisposable.Dispose()
        {
            // intentionally blank
        }
        #endregion

        #region MVC Controller Resolver
        object System.Web.Mvc.IDependencyResolver.GetService(Type serviceType)
        {
            return GetServiceInternal(serviceType);
        }

        IEnumerable<object> System.Web.Mvc.IDependencyResolver.GetServices(Type serviceType)
        {
            return GetServicesInternal(serviceType);
        }
        #endregion

        #region SignalR Resolver
        object Microsoft.AspNet.SignalR.IDependencyResolver.GetService(Type serviceType)
        {
            return GetServiceInternal(serviceType);
        }

        IEnumerable<object> Microsoft.AspNet.SignalR.IDependencyResolver.GetServices(Type serviceType)
        {
            return GetServicesInternal(serviceType);
        }

        //public void Register(Type serviceType, IEnumerable<Func<object>> activators)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Register(Type serviceType, Func<object> activator)
        //{
        //    this.Kernel.Bind(serviceType).ToSelf().InSingletonScope();
        //}
        #endregion

        private object GetServiceInternal(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        private IEnumerable<object> GetServicesInternal(Type serviceType)
        {
            try
            {
                return Kernel.GetAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
