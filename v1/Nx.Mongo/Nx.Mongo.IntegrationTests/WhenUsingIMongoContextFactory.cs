using Ninject;
using NUnit.Framework;

namespace Nx.Mongo.IntegrationTests
{
    [TestFixture]
    public class WhenUsingIMongoContextFactory : TestFixture
    {
        private const string ConnectionStringName = "testdatabase";

        [Test]
        public void ShouldCreateAndDisposeTheIMongoContextFactory()
        {
            using (var contextFactory = Kernel.Get<IMongoContextFactory>())
            {
                Assert.IsNotNull(contextFactory);
            }
        }

        [Test]
        public void ShouldCreateANonNullIMongoContext()
        {
            using (var contextFactory = Kernel.Get<IMongoContextFactory>())
            {
                using (var context = contextFactory.CreateContext<MongoContext>(ConnectionStringName))
                {
                    Assert.IsNotNull(context);
                }
            }
        }
    }
}