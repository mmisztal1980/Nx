using Ninject;
using Nx.Cloud.Blobs;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Nx.Cloud.Tests.Blobs
{
    public class WhenUsingTheBlobRepository : CloudTestFixtureBase
    {
        private int Count = 10;

        public WhenUsingTheBlobRepository()
        {
            DeleteBlobs();
        }

        private IList<TestBlobData> BuildTestData()
        {
            List<TestBlobData> result = new List<TestBlobData>();

            for (int i = 0; i < Count; i++)
            {
                string id = string.Format("ID{0}", i);
                byte[] data = new byte[20];

                result.Add(new TestBlobData(id, data));
            }

            return result;
        }

        public void DeleteBlobs()
        {
            //Assert.IsTrue(StorageEmulatorIsRunning());

            using (var repository = Kernel.Get<IBlobRepository<TestBlobData>>())
            {
                var keysToDelete = repository.GetBlobKeys();
                foreach (string key in keysToDelete)
                {
                    repository.Delete(key);
                }
            }
        }

        [Test]
        public void ShouldCreateAndDisposeTheBlobRepository()
        {
            Assert.DoesNotThrow(() =>
            {
                using (IBlobRepository<TestBlobData> repository = Kernel.Get<IBlobRepository<TestBlobData>>())
                {
                }
            });
        }

        [Test]
        public void ShouldReturnNullForNonExistingKey()
        {
            Assert.DoesNotThrow(() =>
            {
                using (IBlobRepository<TestBlobData> repository = Kernel.Get<IBlobRepository<TestBlobData>>())
                {
                    TestBlobData entity = repository.Get(new Random().Next().ToString());
                    Assert.Null(entity);
                }
            });
        }

        [Test]
        public void ShouldSaveEntities()
        {
            Assert.DoesNotThrow(() =>
            {
                var data = BuildTestData();

                using (IBlobRepository<TestBlobData> repository = Kernel.Get<IBlobRepository<TestBlobData>>())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        repository.Save(data[i]);
                    }

                    Assert.AreEqual(Count, repository.Count);
                }
            });
        }

        [Test]
        public void ShouldDeleteEntities()
        {
            Assert.DoesNotThrow(() =>
            {
                var data = BuildTestData();

                using (IBlobRepository<TestBlobData> repository = Kernel.Get<IBlobRepository<TestBlobData>>())
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
            });
        }

        [Test]
        public void ShouldDeleteEntitesByKey()
        {
            Assert.DoesNotThrow(() =>
            {
                var data = BuildTestData();

                using (IBlobRepository<TestBlobData> repository = Kernel.Get<IBlobRepository<TestBlobData>>())
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
            });
        }

        [Test]
        public void ShouldGetEntites()
        {
            Assert.DoesNotThrow(() =>
            {
                var data = BuildTestData();

                using (IBlobRepository<TestBlobData> repository = Kernel.Get<IBlobRepository<TestBlobData>>())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        repository.Save(data[i]);
                    }

                    Assert.AreEqual(Count, repository.Count);

                    foreach (TestBlobData entity in data)
                    {
                        var size = repository.Get(entity.Id).Size;
                        Assert.AreEqual(entity.Size, size);
                    }
                }
            });
        }
    }
}
