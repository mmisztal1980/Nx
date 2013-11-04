using MongoDB.Bson;
using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Mongo.IntegrationTests
{
    [TestFixture]
    public class WhenUsingTheMongoRepository : TestFixture
    {
        private const int TimeOut = 30000;
        private const string ConnectionStringName = "testdatabase";
        private const string DatabaseName = "nx_mongo_testdatabase";

        [SetUp]
        public void SetUp()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);
                repository.Delete(e => true);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldCreateAndDisposeTheRepository()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var repository = Kernel.Get<IMongoEntityRepository>())
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
            using (var repository = Kernel.Get<IMongoEntityRepository>())
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
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);
                Assert.IsTrue(repository.Initialized);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldSaveAndRetrieveEntity()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                var entity = new MongoEntity() { Name = "Test Entity" };

                var id = repository.Save(entity);

                Assert.AreNotEqual(ObjectId.Empty, id);

                var result = repository.GetOne(e => e.Id.Equals(id));

                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldSaveAndRetrieveEntities()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                var newEntities = new[]
                {
                    new MongoEntity() { Name = "ABC" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "ABC" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "ABC" + Guid.NewGuid().ToString() }
                };

                IEnumerable<ObjectId> ids;
                Assert.AreEqual(newEntities.Length, repository.Save(newEntities, out ids));

                IEnumerable<MongoEntity> entities;
                Assert.AreEqual(newEntities.Length, repository.Get(e => ids.Contains(e.Id), out entities));
                Assert.IsNotNull(entities);
                Assert.AreNotEqual(Enumerable.Empty<MongoEntity>(), entities);
                Assert.IsTrue(ids != null && !ids.Equals(Enumerable.Empty<ObjectId>()));
                Assert.AreEqual(newEntities.Count(), entities.Count());
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldDeleteEntity()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                var entity = new MongoEntity() { Name = "Test Entity" };

                var id = repository.Save(entity);

                Assert.AreNotEqual(ObjectId.Empty, id);

                var result = repository.GetOne(e => e.Id.Equals(id));

                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);

                repository.Delete(result);

                repository.GetOne(e => e.Id.Equals(id));
                Assert.Fail("Should not reach this line");
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldDeleteEntities()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                var newEntities = new[]
                        {
                            new MongoEntity() {Name = "CDB" + Guid.NewGuid().ToString()},
                            new MongoEntity() {Name = "CDB" + Guid.NewGuid().ToString()},
                            new MongoEntity() {Name = "CDB" + Guid.NewGuid().ToString()}
                        };

                IEnumerable<ObjectId> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                IEnumerable<MongoEntity> entities;
                Assert.AreEqual(newEntities.Count(), repository.Get(ids, out entities));
                Assert.IsNotNull(entities);
                Assert.AreNotEqual(Enumerable.Empty<MongoEntity>(), entities);
                Assert.AreEqual(newEntities.Count(), entities.Count());

                Assert.AreEqual(entities.Count(), repository.Delete(entities));

                IEnumerable<MongoEntity> deletedEntities;
                Assert.AreEqual(0, repository.Get(e => ids.Contains(e.Id), out deletedEntities));
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldDeleteEntitiesMatchingPredicate()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                var newEntities = new[]
                {
                    new MongoEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "DEF" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "KOI" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "KOI" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "KOI" + Guid.NewGuid().ToString() },
                    new MongoEntity() { Name = "KOI" + Guid.NewGuid().ToString() }
                };

                IEnumerable<ObjectId> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                Assert.AreEqual(4, repository.Delete(e => e.Name.StartsWith("KOI")));

                IEnumerable<MongoEntity> deletedEntities;
                Assert.AreEqual(0, repository.Get(e => e.Name.StartsWith("KOI"), out deletedEntities));
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldUpdateEntity()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                const string originalName = "originalName";
                const string updatedName = "updatedName";
                const string updatedProperty = "updatedProperty";

                repository.Initialize(ConnectionStringName, DatabaseName);

                var entity = new MongoEntity() { Name = originalName, Property = string.Empty };

                var id = repository.Save(entity);

                Assert.AreNotEqual(ObjectId.Empty, id);

                var result = repository.GetOne(e => e.Id.Equals(id));

                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(originalName, result.Name);

                result.Name = updatedName;
                result.Property = updatedProperty;

                repository.Update(result);

                var updatedResult = repository.GetOne(e => e.Id.Equals(id));

                Assert.IsNotNull(updatedResult);
                Assert.AreEqual(updatedName, updatedResult.Name);
                Assert.AreEqual(updatedProperty, updatedResult.Property);
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldUpdateEntities()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                const string originalName = "originalName";
                const string updatedName = "updatedName";
                const string updatedProperty = "updatedProperty";

                var newEntities = new[]
                        {
                            new MongoEntity() { Name = originalName, Property = string.Empty },
                            new MongoEntity() { Name = originalName, Property = string.Empty },
                            new MongoEntity() { Name = originalName, Property = string.Empty }
                        };

                IEnumerable<ObjectId> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                IEnumerable<MongoEntity> entities;
                Assert.AreEqual(newEntities.Count(), repository.Get(e => ids.Contains(e.Id), out entities));
                Assert.IsNotNull(entities);
                Assert.AreNotEqual(Enumerable.Empty<MongoEntity>(), entities);
                Assert.AreEqual(newEntities.Count(), entities.Count());

                foreach (var entity in entities)
                {
                    entity.Name = updatedName;
                    entity.Property = updatedProperty;
                }

                repository.Update(entities);

                IEnumerable<MongoEntity> updatedEntities;
                Assert.AreEqual(newEntities.Length, repository.Get(e => ids.Contains(e.Id), out updatedEntities));

                foreach (var updatedEntity in updatedEntities)
                {
                    Assert.AreEqual(updatedName, updatedEntity.Name);
                    Assert.AreEqual(updatedProperty, updatedEntity.Property);
                }
            }
        }

        [Test]
        [NUnit.Framework.Timeout(TimeOut)]
        [ExclusivelyUses(ConnectionStringName)]
        public void ShouldPageEntities()
        {
            using (var repository = Kernel.Get<IMongoEntityRepository>())
            {
                repository.Initialize(ConnectionStringName, DatabaseName);

                var newEntities = new List<MongoEntity>();

                const int entitiesCount = 100;

                for (int i = 0; i < entitiesCount; i++)
                {
                    newEntities.Add(new MongoEntity() { Name = "EFG" + Guid.NewGuid().ToString() });
                }

                IEnumerable<ObjectId> ids;
                Assert.AreEqual(newEntities.Count(), repository.Save(newEntities, out ids));

                int totalResultsCount = 0;
                int resultCount = 0;
                int pageIdx = 0;
                const int take = 17;

                IEnumerable<MongoEntity> entities;
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