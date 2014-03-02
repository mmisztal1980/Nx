using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nx.Cloud.Tables
{
    public interface ITableRepository<T> : IDisposable
       where T : TableServiceEntity, new()
    {
        int Count();

        int Count(string partitioningKey);

        void Delete(string partitioningKey);

        void Delete(string partitioningKey, string rowKey);

        Task DeleteAsync(string partitioningKey);

        Task DeleteAsync(string partitioningKey, string rowKey);

        T Get(string partitioningKey, string rowKey);

        IEnumerable<T> Get(string partitioningKey);

        IEnumerable<T> Get(string partitioningKey, int skip, int take);

        IEnumerable<T> Get(string partitioningKey, IEnumerable<string> rowKeys);

        void Insert(string partitioningKey, string rowKey, T value);

        Task InsertAsync(string partitioningKey, string rowKey, T value);

        bool Update(T value);

        Task<bool> UpdateAsync(T value);
    }
}