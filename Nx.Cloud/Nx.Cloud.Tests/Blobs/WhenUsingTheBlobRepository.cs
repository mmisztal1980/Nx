using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.Cloud.Blobs;
using System;
using System.Collections.Generic;

namespace Nx.Cloud.Tests.Blobs
{
    public class WhenUsingTheBlobRepository : CloudTestFixtureBase
    {
        private const string BlobContainerResourceName = "blobContainer";
        private const int Count = 10;

        [SetUp]
        public void SetUp()
        {
            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                var keys = repository.GetBlobKeys();
                foreach (var key in keys)
                {
                    repository.Delete(key);
                }

                var data = BuildTestData();
                for (int i = 0; i < data.Count; i++)
                {
                    repository.Save(data[i]);
                }

                Assert.AreEqual(Count, repository.Count);
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public void ShouldCreateAndDisposeTheBlobRepository()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
                {
                }
            });
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public void ShouldDeleteEntitesByKey()
        {
            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                var keys = repository.GetBlobKeys();

                foreach (var key in keys)
                {
                    repository.Delete(key);
                }

                Assert.AreEqual(0, repository.Count);
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public async void ShouldDeleteEntitesByKeyAsync()
        {
            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                var keys = repository.GetBlobKeys();

                foreach (var key in keys)
                {
                    await repository.DeleteAsync(key);
                }

                Assert.AreEqual(0, repository.Count);
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public void ShouldDeleteEntities()
        {
            var data = BuildTestData();

            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    repository.Save(data[i]);
                }

                Assert.AreEqual(Count, repository.Count);

                foreach (TestBlobData entity in data)
                {
                    repository.Delete(entity.Id);
                }

                Assert.AreEqual(0, repository.Count);
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public void ShouldGetEntites()
        {
            var data = BuildTestData();

            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    repository.Save(data[i]);
                }

                Assert.AreEqual(Count, repository.Count);

                foreach (var entity in data)
                {
                    var size = repository.Get(entity.Id).Size;
                    Assert.AreEqual(entity.Size, size);
                }
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public async void ShouldGetEntitesAsync()
        {
            var data = BuildTestData();

            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    await repository.SaveAsync(data[i]);
                }

                Assert.AreEqual(Count, repository.Count);

                foreach (var entity in data)
                {
                    var size = (await repository.GetAsync(entity.Id)).Size;
                    Assert.AreEqual(entity.Size, size);
                }
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public void ShouldReturnNullForNonExistingKey()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
                {
                    TestBlobData entity = repository.Get(new Random().Next().ToString());
                    Assert.Null(entity);
                }
            });
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public void ShouldSaveEntities()
        {
            var data = BuildTestData();

            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    repository.Save(data[i]);
                }

                Assert.AreEqual(Count, repository.Count);
            }
        }

        [Test]
        [ExclusivelyUses(BlobContainerResourceName)]
        public async void ShouldSaveEntitiesAsync()
        {
            var data = BuildTestData();

            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    await repository.SaveAsync(data[i]);
                }

                Assert.AreEqual(Count, repository.Count);
            }
        }

        private IList<TestBlobData> BuildTestData()
        {
            var result = new List<TestBlobData>();

            for (int i = 0; i < Count; i++)
            {
                string id = string.Format("ID{0}", i);
                byte[] data = new byte[20];

                result.Add(new TestBlobData(id, data));
            }

            return result;
        }
    }
}