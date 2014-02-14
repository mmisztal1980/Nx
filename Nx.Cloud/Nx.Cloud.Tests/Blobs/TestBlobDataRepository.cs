using Microsoft.WindowsAzure.Storage.Blob;
using Nx.Cloud.Blobs;
using Nx.Cloud.Configuration;
using System.IO;

namespace Nx.Cloud.Tests.Blobs
{
    public class TestBlobDataRepository : BlobRepository<TestBlobData>
    {
        public TestBlobDataRepository(ICloudConfiguration config)
            : base(config, "testblobdata")
        {
        }

        protected override TestBlobData GetBlobData(ICloudBlob blob)
        {
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            return new TestBlobData(blob.Name, stream);
        }
    }
}
