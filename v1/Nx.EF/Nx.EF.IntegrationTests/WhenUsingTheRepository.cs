using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.EF.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nx.EF.IntegrationTests
{
    [TestFixture]
    public class WhenUsingTheRepository : TestFixture
    {
        private const string ConnectionStringName = "testDatabase";
        private const int TimeOut = 15000;

        [SetUp]
        public void SetUp()
        {
            // Migrate the database when the model changes
            using (var migrator = Kernel.Get<IDomainMigratorService>())
            {
                migrator.Migrate<ModelContext, Nx.EF.IntegrationTests.Migrations.Configuration>(ConnectionStringName);
            }

            // Clear the repository each time a test starts
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);
                repository.Delete(p => true);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldCreateAndDisposeTheRepository()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var repository = Kernel.Get<ITestEntityRepository>())
                {
                    Assert.IsNotNull(repository);
                }
            });
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowWhenUsingAnUninitializedRepository()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                Logger.Debug("Should throw from this line", repository.Delete(p => true));
                Assert.Fail("TEST FAILED - Should never reach this line");
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldInitialize()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);
                Assert.IsTrue(repository.Initialized);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldSaveAndRetrieveAnEntity()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                int id = -1;
                {
                    var name = Guid.NewGuid().ToString();
                    var newEntity = new TestEntity() { Name = name };

                    id = repository.Save(newEntity);
                }

                Assert.AreNotEqual(-1, id);

                var entity = repository.GetOne(e => e.Id.Equals(id));
                Assert.IsNotNull(entity);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldSaveAndRetrieveEntities()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                var newEntities = new TestEntity[]
                        {
                            new TestEntity() {Name = "ABC" + Guid.NewGuid().ToString()},
                            new TestEntity() {Name = "ABC" + Guid.NewGuid().ToString()},
                            new TestEntity() {Name = "ABC" + Guid.NewGuid().ToString()}
                        };

                IEnumerable<int> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                IEnumerable<TestEntity> entities;
                Assert.AreEqual(newEntities.Count(), repository.Get(e => ids.Contains(e.Id), out entities));
                Assert.IsNotNull(entities);
                Assert.AreNotEqual(Enumerable.Empty<TestEntity>(), entities);
                Assert.AreEqual(newEntities.Count(), entities.Count());
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldDeleteEntity()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                int id = -1;
                {
                    var name = Guid.NewGuid().ToString();
                    var newEntity = new TestEntity() { Name = name };

                    id = repository.Save(newEntity);
                }

                Assert.AreNotEqual(-1, id);

                var entity = repository.GetOne(e => e.Id.Equals(id));
                Assert.IsNotNull(entity);

                repository.Delete(entity);

                var deletedEntity = repository.GetOne(e => e.Id.Equals(id));
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldDeleteEntities()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                var newEntities = new TestEntity[]
                        {
                            new TestEntity() {Name = "CDB" + Guid.NewGuid().ToString()},
                            new TestEntity() {Name = "CDB" + Guid.NewGuid().ToString()},
                            new TestEntity() {Name = "CDB" + Guid.NewGuid().ToString()}
                        };

                IEnumerable<int> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                IEnumerable<TestEntity> entities;
                Assert.AreEqual(newEntities.Count(), repository.Get(e => ids.Contains(e.Id), out entities));
                Assert.IsNotNull(entities);
                Assert.AreNotEqual(Enumerable.Empty<TestEntity>(), entities);
                Assert.AreEqual(newEntities.Count(), entities.Count());

                Assert.AreEqual(entities.Count(), repository.Delete(entities));

                IEnumerable<TestEntity> deletedEntities;
                Assert.AreEqual(0, repository.Get(e => ids.Contains(e.Id), out deletedEntities));
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldDeleteEntitiesMatchingPredicate()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                var newEntities = new[]
                        {
                            new TestEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "EFG" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "EFG" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "EFG" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "EFG" + Guid.NewGuid().ToString() }
                        };

                IEnumerable<int> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                Assert.AreEqual(4, repository.Delete(e => e.Name.StartsWith("EFG")));

                IEnumerable<TestEntity> deletedEntities;
                Assert.AreEqual(0, repository.Get(e => e.Name.StartsWith("EFG"), out deletedEntities));
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldUpdateEntity()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                int id = -1;
                {
                    var name = Guid.NewGuid().ToString();
                    var newEntity = new TestEntity() { Name = name };

                    id = repository.Save(newEntity);
                }

                Assert.AreNotEqual(-1, id);

                var entity = repository.GetOne(e => e.Id.Equals(id));
                Assert.IsNotNull(entity);

                var newName = "FGH" + entity.Name;
                entity.Name = newName;
                repository.Update(entity);

                var updatedEntity = repository.GetOne(e => e.Id.Equals(id));
                Assert.AreEqual(newName, updatedEntity.Name);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldUpdateEntities()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                var newEntities = new[]
                        {
                            new TestEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                            new TestEntity() { Name = "DEF" + Guid.NewGuid().ToString() }
                        };

                IEnumerable<int> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                IEnumerable<TestEntity> entities;
                Assert.AreEqual(ids.Count(), repository.Get(ids, out entities));

                foreach (var entity in entities)
                {
                    entity.Name = entity.Name.Replace("DEF", "FED");
                }

                repository.Update(entities);

                IEnumerable<TestEntity> updatedEntities;
                Assert.AreEqual(ids.Count(), repository.Get(ids, out updatedEntities));

                Assert.IsNotNull(updatedEntities);
                Assert.IsTrue(updatedEntities.All(e => e.Name.StartsWith("FED")));
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldPageEntities()
        {
            using (var repository = Kernel.Get<ITestEntityRepository>())
            {
                repository.Initialize(ConnectionStringName);

                var newEntities = new List<TestEntity>();

                const int entitiesCount = 100;

                for (int i = 0; i < entitiesCount; i++)
                {
                    newEntities.Add(new TestEntity() { Name = "EFG" + Guid.NewGuid().ToString() });
                }

                IEnumerable<int> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                int totalResultsCount = 0;
                int resultCount = 0;
                int pageIdx = 0;
                const int take = 17;

                IEnumerable<TestEntity> entities;
                while ((resultCount = repository.Get(e => e.Name.StartsWith("EFG"), pageIdx * take, take, out entities)) >
                       0)
                {
                    totalResultsCount += resultCount;
                    pageIdx++;
                }

                Assert.AreEqual(entitiesCount, totalResultsCount);
            }
        }
    }
}