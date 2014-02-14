using FizzWare.NBuilder;
using Ninject;
using Nx.Cloud.Tables;
using NUnit.Framework;

namespace Nx.Cloud.Tests.Tables
{
    public class WhenUsingTableRepository : CloudTestFixtureBase
    {
        private int Count = 10;
        private string Key = "PK1";

        [Test]
        public void ShouldCreateAndDisposeTheTableRepository()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
                {
                    Assert.NotNull(repository);
                }
            });
        }

        [Test]
        public void ShouldInsertTableRows()
        {
            Assert.DoesNotThrow(() =>
            {
                DeleteTableRows();

                var data = Builder<TestTableData>.CreateListOfSize(this.Count).Build();

                using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        repository.Insert(Key, string.Format("data_{0}", i), data[i]);
                    }

                    Assert.AreEqual(Count, repository.Count(Key));
                }
            });
        }

        [Test]
        public void ShouldDeleteTableRows()
        {
            Assert.DoesNotThrow(() =>
            {
                ShouldInsertTableRows();
                DeleteTableRows();

                using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
                {
                    Assert.AreEqual(0, repository.Count(Key));
                }
            });
        }

        [Test]
        public void ShouldGetTableRows()
        {
            Assert.DoesNotThrow(() =>
            {
                DeleteTableRows();

                var data = Builder<TestTableData>.CreateListOfSize(this.Count).Build();

                using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        repository.Insert(Key, string.Format("data_{0}", i), data[i]);
                    }

                    Assert.AreEqual(Count, repository.Count(Key));

                    for (int i = 0; i < data.Count; i++)
                    {
                        var result = repository.Get(Key, string.Format("data_{0}", i));

                        Assert.AreEqual(data[i], result);
                    }
                }
            });
        }

        [Test]
        public void ShouldUpdateTableRows()
        {
            Assert.DoesNotThrow(() =>
            {
                DeleteTableRows();

                var data = Builder<TestTableData>.CreateListOfSize(this.Count).Build();

                using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        repository.Insert(Key, string.Format("data_{0}", i), data[i]);
                    }

                    Assert.AreEqual(Count, repository.Count(Key));

                    for (int i = 0; i < data.Count; i++)
                    {
                        var rowKey = string.Format("data_{0}", i);
                        var x = repository.Get(Key, rowKey);

                        x.Data1 = string.Empty;
                        repository.Update(x);
                    }

                    for (int i = 0; i < data.Count; i++)
                    {
                        var result = repository.Get(Key, string.Format("data_{0}", data.Count - i - 1));

                        Assert.AreEqual(string.Empty, result.Data1);
                    }
                }
            });
        }

        [Test]
        public void ShouldReturnNullForNonExistingKey()
        {
            Assert.DoesNotThrow(() =>
            {
                DeleteTableRows();
                var data = Builder<TestTableData>.CreateListOfSize(this.Count).Build();

                using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        repository.Insert(Key, string.Format("data_{0}", i), data[i]);
                    }

                    Assert.AreEqual(Count, repository.Count(Key));

                    for (int i = 0; i < data.Count; i++)
                    {
                        var result = repository.Get(string.Format("data_{0}", i), Key);

                        Assert.Null(result);
                    }
                }
            });
        }

        public void DeleteTableRows()
        {
            Assert.True(this.StorageEmulatorIsRunning);

            using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
            {
                int count = repository.Count(Key);

                for (int i = 0; i < count; i++)
                {
                    repository.Delete(Key, string.Format("data_{0}", i));
                }
            }
        }
    }
}