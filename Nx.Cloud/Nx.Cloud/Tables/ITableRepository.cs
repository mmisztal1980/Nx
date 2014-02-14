using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;

namespace Nx.Cloud.Tables
{
    public interface ITableRepository<T> : IDisposable
       where T : TableServiceEntity, new()
    {
        T Get(string partitioningKey, string rowKey);

        IEnumerable<T> Get(string partitioningKey);

        IEnumerable<T> Get(string partitioningKey, int skip, int take);

        IEnumerable<T> Get(string partitioningKey, IEnumerable<string> rowKeys);

        void Insert(string partitioningKey, string rowKey, T value);

        void Delete(string partitioningKey, string rowKey);

        bool Update(T value);

        int Count();

        int Count(string partitioningKey);
    }
}
