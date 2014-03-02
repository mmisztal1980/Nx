using Microsoft.WindowsAzure.Storage.Blob;
using Nx.Cloud.Blobs;
using Nx.Cloud.Configuration;
using Nx.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Nx.Cloud.Concurrency
{
    internal class CloudLockBlobRepository : BlobRepository<CloudLockBlobData>
    {
        public CloudLockBlobRepository(ILogFactory logFactory, ICloudConfiguration config)
            : base(logFactory, config, "cloudlocks")
        {
        }

        protected override CloudLockBlobData GetBlobData(ICloudBlob blob)
        {
            var data = new MemoryStream();
            blob.DownloadToStream(data);
            return new CloudLockBlobData(blob.Name, data);
        }

        protected async override Task<CloudLockBlobData> GetBlobDataAsync(ICloudBlob blob)
        {
            var data = new MemoryStream();
            await blob.DownloadToStreamAsync(data);
            return new CloudLockBlobData(blob.Name, data);
        }
    }
}