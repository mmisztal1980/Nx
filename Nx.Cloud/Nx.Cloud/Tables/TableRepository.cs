using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Nx.Cloud.Configuration;
using Nx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nx.Cloud.Tables
{
    /// <summary>
    /// The abstract implementation of a generic TableRepository.
    /// Recommend creation via IoC container. Requires ILogFactory & ICloudConfiguration registrations in the IoC container
    /// </summary>
    /// <typeparam name="T">Type of the TableService entity</typeparam>
    public abstract class TableRepository<T> : ITableRepository<T>
      where T : TableServiceEntity, new()
    {
        private readonly object _lock = new object();
        private readonly ServiceContext Context;
        private readonly ILogger Logger;

        public TableRepository(ICloudConfiguration config, string tableName)
        {
            Logger = new NullLogger("TableRepository");
            Context = new ServiceContext(config, tableName);
        }

        #region IDisposable Members

        ~TableRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                }

                if (Logger != null)
                {
                    Logger.Dispose();
                }
            }
        }

        #endregion IDisposable Members

        #region ITableRepository<T> Members

        public int Count()
        {
            return Context.Entries.Count();
        }

        public int Count(string partitioningKey)
        {
            return Get(partitioningKey).Count();
        }

        public void Delete(string partitioningKey, string rowKey)
        {
            Logger.Debug("Deleting row : {0} / {1}", partitioningKey, rowKey);
            var item = Get(partitioningKey, rowKey);
            if (item != null)
            {
                Context.DeleteObject(item);
                Context.SaveChangesWithRetries();
            }
            else
            {
                Logger.Error("Failed to delete row {0} / {1}", partitioningKey, rowKey);
            }
        }

        public T Get(string partitioningKey, string rowKey)
        {
            try
            {
                Logger.Debug("Retrieving row : {0} / {1}", partitioningKey, rowKey);
                return Context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey) && le.RowKey.Equals(rowKey)).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public IEnumerable<T> Get(string partitioningKey)
        {
            Logger.Debug("Retrieving rows : {0} / *", partitioningKey);
            return Context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey)).AsEnumerable();
        }

        public IEnumerable<T> Get(string partitioningKey, int skip, int take)
        {
            Logger.Debug("Retrieving rows : {0} / skip : {1} take {2}", partitioningKey, skip, take);
            return Context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey)).Skip(skip).Take(take).AsEnumerable();
        }

        public IEnumerable<T> Get(string partitioningKey, IEnumerable<string> rowKeys)
        {
            Logger.Debug("Retrieving rows : {0} / (...)", partitioningKey);
            return Context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey) && rowKeys.Contains(le.RowKey)).AsEnumerable();
        }

        public void Insert(string partitioningKey, string rowKey, T value)
        {
            lock (_lock)
            {
                Logger.Debug("Inserting row : {0} / {1}", partitioningKey, rowKey);
                value.PartitionKey = partitioningKey;
                value.RowKey = rowKey;

                Context.AddObject(Context.TableName, value);
                Context.SaveChangesWithRetries();
            }
        }

        public bool Update(T value)
        {
            lock (_lock)
            {
                bool result = true;
                try
                {
                    Context.UpdateObject(value);
                    Logger.Debug("Updated row : {0} / {1}", value.PartitionKey, value.RowKey);
                    Context.SaveChangesWithRetries();
                }
                catch (Exception ex)
                {
                    ex.GetType();
                    Logger.Error("Failed to update row : {0} / {1}", value.PartitionKey, value.RowKey);
                    result = false;
                }

                return result;
            }
        }

        #endregion ITableRepository<T> Members

        private void Copy(T from, T to)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if ((!property.Name.Equals("PartitioningKey")) && (!property.Name.Equals("RowKey")))
                {
                    dynamic value = property.GetValue(from, null);
                    property.SetValue(to, value, null);
                }
            }
        }

        #region Nested Types

        private class ServiceContext : TableServiceContext, IDisposable
        {
            public string TableName;

            public ServiceContext(ICloudConfiguration config, string tableName)
                : base(config.StorageAccount.CreateCloudTableClient())
            {
                TableName = tableName;

                this.ServiceClient.GetTableReference(tableName)
                    .CreateIfNotExists(new TableRequestOptions()
                    {
                        RetryPolicy = config.GlobalRetryPolicy
                    }, null);
            }

            #region IDisposable Members

            ~ServiceContext()
            {
                Dispose(false);
            }

            #endregion IDisposable Members

            public IQueryable<T> Entries
            {
                get
                {
                    return this.CreateQuery<T>(TableName).AsTableServiceQuery<T>(this);
                }
            }
        }

        #endregion Nested Types
    }
}