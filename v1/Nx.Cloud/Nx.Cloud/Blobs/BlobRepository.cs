using Microsoft.WindowsAzure.Storage.Blob;
using Nx.Cloud.Configuration;
using Nx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nx.Cloud.Blobs
{
    public abstract class BlobRepository<T> : IBlobRepository<T>
        where T : class, IBlobData
    {
        private readonly CloudBlobContainer _container;
        private readonly string _containerName;
        private readonly ILogger _logger;

        protected BlobRepository(ILogFactory logFactory, ICloudConfiguration config, string containerName)
            : this(logFactory, config, containerName, BlobContainerPublicAccessType.Off)
        {
        }

        protected BlobRepository(ILogFactory logFactory, ICloudConfiguration config, string containerName, BlobContainerPublicAccessType permission)
            : this(logFactory)
        {
            _containerName = containerName;
            _container = GetContainer(config, _containerName, permission);
        }

        private BlobRepository(ILogFactory logFactory)
        {
            _logger = logFactory.CreateLogger("BlobRepository");
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

        public long Count
        {
            get
            {
                return _container.ListBlobs().LongCount();
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

        public async Task AppendAsync(string key, byte[] data)
        {
            _logger.Debug("Appending to blob[{0}], {1} [bytes]", key, data.Length);
            var blob = GetBlob(key);

            using (var stream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(stream);
                await stream.WriteAsync(data, 0, data.Length);
                await blob.UploadFromStreamAsync(stream);
                stream.Close();
            }
        }

        public void Delete(string key)
        {
            _logger.Debug("Deleting blob[{0}]", key);
            var blob = GetBlob(key);
            blob.DeleteIfExists();
        }

        public async Task DeleteAsync(string key)
        {
            _logger.Debug("Deleting blob[{0}]", key);
            var blob = GetBlob(key);
            await blob.DeleteIfExistsAsync();
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

        public async Task<T> GetAsync(string key)
        {
            try
            {
                return await GetBlobDataAsync(GetBlob(key));
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

        public long GetBlobKeys(out IEnumerable<string> keys)
        {
            keys = _container.ListBlobs().Select(bi => bi.Uri.ToString());
            return keys.LongCount();
        }

        public long GetBlobKeys(int pageIdx, int pageSize, out IEnumerable<string> keys)
        {
            keys = _container.ListBlobs().Skip(pageIdx - 1).Take(pageSize).Select(bi => bi.Uri.ToString());
            return keys.LongCount();
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

        public async Task SaveAsync(T data)
        {
            _logger.Debug("Saving blob[{0}], {1} [bytes]", data.Id, data.Size);
            var blob = GetBlob(data.Id);

            blob.Properties.ContentType = data.MetaData["ContentType"];

            foreach (var kvp in data.MetaData)
            {
                blob.Metadata.Add(kvp.Key, kvp.Value);
            }

            await blob.UploadFromStreamAsync(data.Data);
        }

        protected abstract T GetBlobData(ICloudBlob blob);

        protected abstract Task<T> GetBlobDataAsync(ICloudBlob blob);

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