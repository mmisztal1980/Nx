using Microsoft.AspNet.SignalR;
using Ninject;
using Nx.Bootstrappers;
using Nx.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Nx.Extensions
{
    /// <summary>
    /// NxWebExtension Makes Ninject the primary dependency resolver for MVC controlers, WebAPI controllers and SignalR hubs
    /// </summary>
    public class NxWebExtension : IBootstrapperExtension
    {
        public void Extend(IKernel kernel)
        {
            // Bind the universal resolver to all existing IDependencyResolver interfaces
            kernel.Bind<
                System.Web.Mvc.IDependencyResolver,
                System.Web.Http.Dependencies.IDependencyResolver,
                Microsoft.AspNet.SignalR.IDependencyResolver>()
                .To<UniversalDependencyResolver>().InSingletonScope();

            // Configure the dependency resolvers to use the Universal dependency resolver
            GlobalConfiguration.Configuration.DependencyResolver = kernel.Get<System.Web.Http.Dependencies.IDependencyResolver>();
            DependencyResolver.SetResolver(kernel.Get<System.Web.Mvc.IDependencyResolver>());
            GlobalHost.DependencyResolver = kernel.Get<Microsoft.AspNet.SignalR.IDependencyResolver>();
        }

        public string Name
        {
            get { return "Nx.Web Extension"; }
        }
    }
}
