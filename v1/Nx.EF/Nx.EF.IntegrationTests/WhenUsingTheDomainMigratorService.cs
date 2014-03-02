using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.EF.IntegrationTests.Migrations;
using Nx.EF.Migrations;
using Palmer;
using System.Data.SqlClient;

namespace Nx.EF.IntegrationTests
{
    [TestFixture]
    [Ignore]
    public class WhenUsingTheDomainMigrator : TestFixture
    {
        private const string ConnectionStringName = "testMigrationsDatabase";
        private const int SetupAttemptCount = 10;
        private const int TimeOut = 15000;

        [SetUp]
        public void SetUp()
        {
            int counter = 0;

            Assert.DoesNotThrow(() =>
                Retry.On<SqlException>()
                .For(SetupAttemptCount)
                .With(ctx =>
                {
                    counter++;
                    Logger.Debug("Attempting to drop the database before the test [{0}/{1}]", counter, SetupAttemptCount);

                    using (var mctx = new ModelContext(ConnectionStringName))
                    {
                        if (mctx.Database.Exists())
                        {
                            mctx.Database.Delete();
                        }
                    }
                }));
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldMigrateTheDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var migrator = Kernel.Get<IDomainMigratorService>())
                {
                    Assert.IsTrue(migrator.Migrate<ModelContext, Configuration>(ConnectionStringName));
                }
            });

            using (var ctx = new ModelContext(ConnectionStringName))
            {
                Assert.IsTrue(ctx.Database.Exists());
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldDropTheDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var migrator = Kernel.Get<IDomainMigratorService>())
                using (var ctx = new ModelContext(ConnectionStringName))
                {
                    migrator.Migrate<ModelContext, Configuration>(ConnectionStringName);

                    Assert.IsTrue(ctx.Database.Exists());

                    Assert.IsTrue(migrator.Drop<ModelContext>(ConnectionStringName));

                    Assert.IsFalse(ctx.Database.Exists());
                }
            });
        }
    }
}