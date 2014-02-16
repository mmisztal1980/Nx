using FizzWare.NBuilder;
using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.Cloud.Tables;

namespace Nx.Cloud.Tests.Tables
{
    public class WhenUsingTableRepository : CloudTestFixtureBase
    {
        private const int Count = 10;
        private const string Key = "PK1";
        private const string TableResourceName = "Table";

        [SetUp]
        public void SetUp()
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

        [Test]
        [ExclusivelyUses(TableResourceName)]
        public void ShouldCreateAndDisposeTheTableRepository()
        {
            using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
            {
                Assert.NotNull(repository);
            }
        }

        [Test]
        [ExclusivelyUses(TableResourceName)]
        public void ShouldDeleteTableRows()
        {
            ShouldInsertTableRows();

            using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
            {
                repository.Delete(Key);
                Assert.AreEqual(0, repository.Count(Key));
            }
        }

        [Test]
        [ExclusivelyUses(TableResourceName)]
        public void ShouldGetTableRows()
        {
            var data = Builder<TestTableData>.CreateListOfSize(Count).Build();

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
        }

        [Test]
        [ExclusivelyUses(TableResourceName)]
        public void ShouldInsertTableRows()
        {
            var data = Builder<TestTableData>.CreateListOfSize(Count).Build();

            using (var repository = Kernel.Get<ITableRepository<TestTableData>>())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    repository.Insert(Key, string.Format("data_{0}", i), data[i]);
                }

                Assert.AreEqual(Count, repository.Count(Key));
            }
        }

        [Test]
        [ExclusivelyUses(TableResourceName)]
        public void ShouldReturnNullForNonExistingKey()
        {
            var data = Builder<TestTableData>.CreateListOfSize(Count).Build();

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
        }

        [Test]
        [ExclusivelyUses(TableResourceName)]
        public void ShouldUpdateTableRows()
        {
            var data = Builder<TestTableData>.CreateListOfSize(Count).Build();

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
        }
    }
}