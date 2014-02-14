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
        private string ContainerName;
        private CloudBlobContainer Container;
        private ILogger Logger;

        public BlobRepository(ICloudConfiguration config, string containerName)
            : this(config, containerName, BlobContainerPublicAccessType.Off)
        {
        }

        public BlobRepository(ICloudConfiguration config, string containerName, BlobContainerPublicAccessType permission)
        {
            Logger = LogFactory.Instance.CreateLogger("BlobRepository");
            ContainerName = containerName;
            Container = GetContainer(config, ContainerName, permission);
        }

        public int Count
        {
            get
            {
                return Container.ListBlobs().Count();
            }
        }

        ~BlobRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Container = null;

                if (Logger != null)
                {
                    Logger.Dispose();
                    Logger = null;
                }
            }
        }

        public void Save(T data)
        {
            Logger.Debug("Saving blob[{0}], {1} [bytes]", data.Id, data.Size);
            ICloudBlob blob = GetBlob(data.Id);

            blob.Properties.ContentType = data.MetaData["ContentType"];

            foreach (var kvp in data.MetaData)
            {
                blob.Metadata.Add(kvp.Key, kvp.Value);
            }

            blob.UploadFromStream(data.Data);
        }

        public void Append(string key, byte[] data)
        {
            Logger.Debug("Appending to blob[{0}], {1} [bytes]", key, data.Length);
            ICloudBlob blob = GetBlob(key);

            using (MemoryStream stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                stream.Write(data, 0, data.Length);
                blob.UploadFromStream(stream);
                stream.Close();
            }
        }

        public T Get(string key)
        {
            try
            {
                return GetBlobData(GetBlob(key));
            }
            catch
            {
                Logger.Error("Requested blob[{0}] not found", key);
                return null;
            }
        }

        public ICloudBlob GetBlob(string key)
        {
            Logger.Debug("Retrieving blob[{0}]", key);
            return Container.GetBlockBlobReference(key);
        }

        public void Delete(string key)
        {
            Logger.Debug("Deleting blob[{0}]", key);
            ICloudBlob blob = GetBlob(key);
            blob.DeleteIfExists();
        }

        public IEnumerable<string> GetBlobKeys()
        {
            return Container.ListBlobs().Select(bi => bi.Uri.ToString());
        }

        protected abstract T GetBlobData(ICloudBlob blob);

        private static CloudBlobContainer GetContainer(ICloudConfiguration config, string containerName, BlobContainerPublicAccessType accessType)
        {
            CloudBlobClient client = config.StorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(containerName);
            container.CreateIfNotExists(new BlobRequestOptions() { RetryPolicy = config.GlobalRetryPolicy }, null);

            BlobContainerPermissions permissions = container.GetPermissions();
            permissions.PublicAccess = accessType;
            container.SetPermissions(permissions);

            return container;
        }
    }
}
