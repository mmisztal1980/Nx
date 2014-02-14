using Microsoft.WindowsAzure.Storage.Blob;
using Nx.Cloud.Blobs;
using Nx.Cloud.Configuration;
using System.IO;

namespace Nx.Cloud.Concurrency
{
    public class CloudLockBlobRepository : BlobRepository<CloudLockBlobData>
    {
        public CloudLockBlobRepository(ICloudConfiguration config)
            : base(config, "cloudlocks")
        {
        }

        protected override CloudLockBlobData GetBlobData(ICloudBlob blob)
        {
            MemoryStream data = new MemoryStream();
            blob.DownloadToStream(data);
            return new CloudLockBlobData(blob.Name, data);
        }
    }
}
