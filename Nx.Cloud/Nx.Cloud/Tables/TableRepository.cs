using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Nx.Cloud.Configuration;
using Nx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly ServiceContext _context;
        private readonly ILogger _logger;

        protected TableRepository(ILogFactory logFactory, ICloudConfiguration config, string tableName)
            : this(logFactory)
        {
            _context = new ServiceContext(config, tableName);
        }

        private TableRepository(ILogFactory logFactory)
        {
            _logger = logFactory.CreateLogger("TableRepository");
        }

        ~TableRepository()
        {
            Dispose(false);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }

                if (_logger != null)
                {
                    _logger.Dispose();
                }
            }
        }

        #endregion IDisposable Members

        #region ITableRepository<T> Members

        public int Count()
        {
            return _context.Entries.Count();
        }

        public int Count(string partitioningKey)
        {
            return Get(partitioningKey).Count();
        }

        public void Delete(string partitioningKey, string rowKey)
        {
            _logger.Debug("Deleting row : {0} / {1}", partitioningKey, rowKey);
            var item = Get(partitioningKey, rowKey);
            if (item != null)
            {
                _context.DeleteObject(item);
                _context.SaveChangesWithRetries();
            }
            else
            {
                _logger.Error("Failed to delete row {0} / {1}", partitioningKey, rowKey);
            }
        }

        public async Task DeleteAsync(string partitioningKey, string rowKey)
        {
            _logger.Debug("Deleting row : {0} / {1}", partitioningKey, rowKey);
            var item = Get(partitioningKey, rowKey);
            if (item != null)
            {
                _context.DeleteObject(item);
                await _context.SaveChangesWithRetriesAsync();
            }
            else
            {
                _logger.Error("Failed to delete row {0} / {1}", partitioningKey, rowKey);
            }
        }

        public T Get(string partitioningKey, string rowKey)
        {
            try
            {
                _logger.Debug("Retrieving row : {0} / {1}", partitioningKey, rowKey);
                return _context.Entries.SingleOrDefault(le => le.PartitionKey.Equals(partitioningKey) && le.RowKey.Equals(rowKey));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public IEnumerable<T> Get(string partitioningKey)
        {
            _logger.Debug("Retrieving rows : {0} / *", partitioningKey);
            return _context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey)).AsEnumerable();
        }

        public IEnumerable<T> Get(string partitioningKey, int skip, int take)
        {
            _logger.Debug("Retrieving rows : {0} / skip : {1} take {2}", partitioningKey, skip, take);
            return _context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey)).Skip(skip).Take(take).AsEnumerable();
        }

        public IEnumerable<T> Get(string partitioningKey, IEnumerable<string> rowKeys)
        {
            _logger.Debug("Retrieving rows : {0} / (...)", partitioningKey);
            return _context.Entries.Where(le => le.PartitionKey.Equals(partitioningKey) && rowKeys.Contains(le.RowKey)).AsEnumerable();
        }

        public void Insert(string partitioningKey, string rowKey, T value)
        {
            _logger.Debug("Inserting row : {0} / {1}", partitioningKey, rowKey);
            value.PartitionKey = partitioningKey;
            value.RowKey = rowKey;

            _context.AddObject(_context.TableName, value);
            _context.SaveChangesWithRetries();
        }

        public async Task InsertAsync(string partitioningKey, string rowKey, T value)
        {
            _logger.Debug("Inserting row : {0} / {1}", partitioningKey, rowKey);
            value.PartitionKey = partitioningKey;
            value.RowKey = rowKey;

            _context.AddObject(_context.TableName, value);
            await _context.SaveChangesWithRetriesAsync();
        }

        public bool Update(T value)
        {
            bool result = true;
            try
            {
                _context.UpdateObject(value);
                _logger.Debug("Updated row : {0} / {1}", value.PartitionKey, value.RowKey);
                _context.SaveChangesWithRetries();
            }
            catch (Exception ex)
            {
                ex.GetType();
                _logger.Error("Failed to update row : {0} / {1}", value.PartitionKey, value.RowKey);
                result = false;
            }

            return result;
        }

        public async Task<bool> UpdateAsync(T value)
        {
            bool result = true;
            try
            {
                _context.UpdateObject(value);
                _logger.Debug("Updated row : {0} / {1}", value.PartitionKey, value.RowKey);
                await _context.SaveChangesWithRetriesAsync();
            }
            catch (Exception ex)
            {
                ex.GetType();
                _logger.Error("Failed to update row : {0} / {1}", value.PartitionKey, value.RowKey);
                result = false;
            }

            return result;
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
            public readonly string TableName;

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