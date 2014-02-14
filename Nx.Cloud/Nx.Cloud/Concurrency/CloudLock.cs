using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Ninject;
using Nx.Cloud.Blobs;
using System;
using System.Threading;

namespace Nx.Cloud.Concurrency
{
    /// <summary>
    /// The Cloud lock aquires a 30s lease on a dedicated blob, and keeps it alive every 30s until it's disposed.
    /// </summary>
    public class CloudLock : IDisposable
    {
        /// <summary>
        /// Entry point for the CloudLock Syntax
        /// </summary>
        /// <param name="lockName">name of the lock</param>
        /// <returns>A named CloudLock</returns>
        public static CloudLock Named(string lockName)
        {
            return new CloudLock(lockName);
        }

        private IBlobRepository<CloudLockBlobData> BlobRepository;
        private ICloudBlob Blob;
        private string leaseId;
        private Thread LeaseRenewalThread;

        public bool HasLock
        {
            get
            {
                return !string.IsNullOrEmpty(leaseId);
            }
        }

        public CloudLock(string lockName)
        {
            IKernel kernel = ServiceLocator.Current.GetInstance<IKernel>();
            BlobRepository = kernel.Get<IBlobRepository<CloudLockBlobData>>();
            Blob = EnsureBlobExists(lockName);

            try
            {
                leaseId = Blob.AcquireLease(TimeSpan.FromSeconds(30), Guid.NewGuid().ToString());
                LeaseRenewalThread = new Thread(() =>
                {
                    try
                    {
                        while (true)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(30));
                            Blob.ReleaseLease(new AccessCondition() { LeaseId = leaseId });
                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                });
                LeaseRenewalThread.Start();
            }
            catch (StorageException)
            {
                leaseId = null;
            }
        }

        private ICloudBlob EnsureBlobExists(string lockName)
        {
            CloudLockBlobData blobData = BlobRepository.Get(lockName);
            {
                if (blobData == null)
                {
                    blobData = new CloudLockBlobData(lockName, new byte[0]);
                    BlobRepository.Save(blobData);
                }
            }
            blobData.Dispose();

            return BlobRepository.GetBlob(lockName);
        }

        ~CloudLock()
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
                if (BlobRepository != null)
                {
                    BlobRepository.Dispose();
                }
            }

            // CleanUp regardless whether Dispose() was called
            if (HasLock)
            {
                LeaseRenewalThread.Abort();
                Blob.ReleaseLease(new AccessCondition() { LeaseId = leaseId });
            }

            Blob = null;
            LeaseRenewalThread = null;
        }
    }
}
