using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;

namespace Nx.Cloud.Blobs
{
    public interface IBlobRepository<T> : IDisposable
        where T : IBlobData
    {
        void Save(T data);

        void Append(string key, byte[] data);

        T Get(string key);

        ICloudBlob GetBlob(string key);

        int Count { get; }

        string ContainerName { get; }

        IEnumerable<string> GetBlobKeys();

        void Delete(string key);
    }
}