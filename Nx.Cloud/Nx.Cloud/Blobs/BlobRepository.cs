using Microsoft.WindowsAzure.Storage.Blob;
using Nx.Cloud.Configuration;
using Nx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nx.Cloud.Blobs
{
    public abstract class BlobRepository<T> : IBlobRepository<T>
        where T : class, IBlobData
    {
        private readonly CloudBlobContainer _container;
        private readonly string _containerName;
        private readonly ILogger _logger;

        protected BlobRepository(ICloudConfiguration config, string containerName)
            : this(config, containerName, BlobContainerPublicAccessType.Off)
        {
        }

        protected BlobRepository(ICloudConfiguration config, string containerName, BlobContainerPublicAccessType permission)
        {
            _logger = new NullLogger("Repository");
            _containerName = containerName;
            _container = GetContainer(config, _containerName, permission);
        }

        ~BlobRepository()
        {
            Dispose(false);
        }

        public string ContainerName
        {
            get
            {
                return _containerName;
            }
        }

        public int Count
        {
            get
            {
                return _container.ListBlobs().Count();
            }
        }

        public void Append(string key, byte[] data)
        {
            _logger.Debug("Appending to blob[{0}], {1} [bytes]", key, data.Length);
            var blob = GetBlob(key);

            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                stream.Write(data, 0, data.Length);
                blob.UploadFromStream(stream);
                stream.Close();
            }
        }

        public void Delete(string key)
        {
            _logger.Debug("Deleting blob[{0}]", key);
            var blob = GetBlob(key);
            blob.DeleteIfExists();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Get(string key)
        {
            try
            {
                return GetBlobData(GetBlob(key));
            }
            catch
            {
                _logger.Error("Requested blob[{0}] not found", key);
                return null;
            }
        }

        public ICloudBlob GetBlob(string key)
        {
            _logger.Debug("Retrieving blob[{0}]", key);
            return _container.GetBlockBlobReference(key);
        }

        public IEnumerable<string> GetBlobKeys()
        {
            return _container.ListBlobs().Select(bi => bi.Uri.ToString());
        }

        public void Save(T data)
        {
            _logger.Debug("Saving blob[{0}], {1} [bytes]", data.Id, data.Size);
            var blob = GetBlob(data.Id);

            blob.Properties.ContentType = data.MetaData["ContentType"];

            foreach (var kvp in data.MetaData)
            {
                blob.Metadata.Add(kvp.Key, kvp.Value);
            }

            blob.UploadFromStream(data.Data);
        }

        protected abstract T GetBlobData(ICloudBlob blob);

        private static CloudBlobContainer GetContainer(ICloudConfiguration config, string containerName, BlobContainerPublicAccessType accessType)
        {
            var client = config.StorageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);
            container.CreateIfNotExists(new BlobRequestOptions() { RetryPolicy = config.GlobalRetryPolicy });

            var permissions = container.GetPermissions();
            permissions.PublicAccess = accessType;
            container.SetPermissions(permissions);

            return container;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_logger != null)
                {
                    _logger.Dispose();
                }
            }
        }
    }
}