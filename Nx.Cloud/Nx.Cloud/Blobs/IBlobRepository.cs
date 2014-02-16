using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nx.Cloud.Blobs
{
    public interface IBlobRepository<T> : IDisposable
        where T : IBlobData
    {
        string ContainerName { get; }

        long Count { get; }

        void Append(string key, byte[] data);

        Task AppendAsync(string key, byte[] data);

        void Delete(string key);

        Task DeleteAsync(string key);

        T Get(string key);

        Task<T> GetAsync(string key);

        ICloudBlob GetBlob(string key);

        IEnumerable<string> GetBlobKeys();

        long GetBlobKeys(out IEnumerable<string> keys);

        long GetBlobKeys(int pageIdx, int pageSize, out IEnumerable<string> keys);

        void Save(T data);

        Task SaveAsync(T data);
    }
}