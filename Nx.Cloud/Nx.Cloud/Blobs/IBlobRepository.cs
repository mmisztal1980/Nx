using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nx.Cloud.Blobs
{
    public interface IBlobRepository<T> : IDisposable
        where T : IBlobData
    {
        void Save(T data);

        Task SaveAsync(T data);

        void Append(string key, byte[] data);

        Task AppendAsync(string key, byte[] data);

        T Get(string key);

        ICloudBlob GetBlob(string key);

        int Count { get; }

        string ContainerName { get; }

        IEnumerable<string> GetBlobKeys();

        void Delete(string key);
    }
}