using System;
using System.Collections.Generic;
using System.IO;

namespace Nx.Cloud.Blobs
{
    public interface IBlobData : IDisposable
    {
        string Id { get; }
        long Size { get; }
        Stream Data { get; }
        ContentType ContentType { get; }
        IDictionary<string, string> MetaData { get; }
    }

    public interface IBlobData<T> : IBlobData
        where T : class
    {
    }
}
