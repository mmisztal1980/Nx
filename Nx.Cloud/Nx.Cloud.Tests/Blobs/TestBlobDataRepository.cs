using Microsoft.WindowsAzure.Storage.Blob;
using Nx.Cloud.Blobs;
using Nx.Cloud.Configuration;
using Nx.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Nx.Cloud.Tests.Blobs
{
    public class TestBlobDataRepository : BlobRepository<TestBlobData>
    {
        public TestBlobDataRepository(ILogFactory logFactory, ICloudConfiguration config)
            : base(logFactory, config, "testblobdata")
        {
        }

        protected override TestBlobData GetBlobData(ICloudBlob blob)
        {
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            return new TestBlobData(blob.Name, stream);
        }

        protected async override Task<TestBlobData> GetBlobDataAsync(ICloudBlob blob)
        {
            var stream = new MemoryStream();
            await blob.DownloadToStreamAsync(stream);
            return new TestBlobData(blob.Name, stream);
        }
    }
}