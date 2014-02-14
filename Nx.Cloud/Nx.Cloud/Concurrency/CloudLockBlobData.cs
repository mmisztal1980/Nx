using Nx.Cloud.Blobs;
using System.IO;

namespace Nx.Cloud.Concurrency
{
    internal class CloudLockBlobData : BlobData<CloudLockBlobData>
    {
        public CloudLockBlobData(string id, byte[] data)
            : base(id, data, ContentType.Binary)
        {
        }

        public CloudLockBlobData(string id, Stream data)
            : base(id, data, ContentType.Binary)
        {
        }
    }
}