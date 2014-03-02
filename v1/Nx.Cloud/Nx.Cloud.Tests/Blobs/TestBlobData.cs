using Nx.Cloud.Blobs;
using System.IO;

namespace Nx.Cloud.Tests.Blobs
{
    public class TestBlobData : BlobData<TestBlobData>
    {
        public TestBlobData(string id, byte[] data)
            : base(id, data, ContentType.Binary)
        {
        }

        public TestBlobData(string id, Stream data)
            : base(id, data, ContentType.Binary)
        {
        }
    }
}
