using Ninject;
using NUnit.Framework;
using Nx.Extensions;
using Nx.Kernel;

namespace Nx.Web.Tests
{
    [TestFixture]
    public class WhenUsingNxWebExtension
    {
        [Test]
        public void MvcDependencyResolverShouldBeRegistered()
        {
            using (var bootstrapper = new Bootstrapper()
                .ExtendBy<NxWebExtension>())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<System.Web.Mvc.IDependencyResolver>());
            }
        }

        [Test]
        public void MvcWebApiDependencyResolverShouldBeRegistered()
        {
            using (var bootstrapper = new Bootstrapper()
                .ExtendBy<NxWebExtension>())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<System.Web.Http.Dependencies.IDependencyResolver>());
            }
        }

        [Test]
        public void SignalRDependencyResolverShouldBeRegistered()
        {
            using (var bootstrapper = new Bootstrapper()
    .ExtendBy<NxWebExtension>())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<Microsoft.AspNet.SignalR.IDependencyResolver>());
            }
        }

        [Test]
        public void AllResolversShouldPointToUniversalDependencyResolverInstance()
        {
            using (var bootstrapper = new Bootstrapper()
    .ExtendBy<NxWebExtension>())
            using (var kernel = bootstrapper.Run())
            {
                var @mvc = kernel.Get<System.Web.Mvc.IDependencyResolver>();
                var @webapi = kernel.Get<System.Web.Http.Dependencies.IDependencyResolver>();
                var @signalR = kernel.Get<Microsoft.AspNet.SignalR.IDependencyResolver>();

                Assert.IsNotNull(@mvc);
                Assert.IsNotNull(@webapi);
                Assert.IsNotNull(@signalR);

                Assert.AreEqual(@mvc.GetHashCode(), @webapi.GetHashCode());
                Assert.AreEqual(@mvc.GetHashCode(), @signalR.GetHashCode());
            }
        }
    }
}
