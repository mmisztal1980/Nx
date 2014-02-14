using Microsoft.WindowsAzure.Storage.Queue;
using Nx.Cloud.Configuration;
using Nx.Cloud.Internals;
using Nx.Logging;
using System;

namespace Nx.Cloud.Queues
{
    public class QueueService<T> : IQueueService<T>
        where T : class, new()
    {
        private ILogger _Logger;
        private CloudQueue _Queue;

        public int Length
        {
            get
            {
                _Queue.FetchAttributes();
                return _Queue.ApproximateMessageCount ?? 0;
            }
        }

        public void Enqueue(T data)
        {
            Condition.Require<ArgumentException>(data != null);

            _Logger.Debug("Enqueueing item");
            _Queue.AddMessage(new CloudQueueMessage(SerializationHelper<T>.SerializeToByteArray(data)));
        }

        public T Dequeue()
        {
            T result;
            _Logger.Debug("Attempting to dequeue item");
            CloudQueueMessage message = _Queue.GetMessage();
            if (message != null)
            {
                result = SerializationHelper<T>.Deserialize(message.AsBytes);
                _Queue.DeleteMessage(message);
                _Logger.Debug("Item dequeued");
            }
            else
            {
                _Logger.Debug("There was no item in the queue");
                result = null;
            }

            return result;
        }

        public void Clear()
        {
            _Queue.Clear();
        }

        public void Delete()
        {
            _Queue.Delete();
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
                _Queue = null;
                _Logger.Dispose();
                OnDisposing();
            }
        }

        protected virtual void OnDisposing()
        {
        }

        public QueueService(ICloudConfiguration config, string queueName)
        {
            _Logger = LogFactory.Instance.CreateLogger("QueueService");
            var queueClient = config.StorageAccount.CreateCloudQueueClient();
            _Queue = queueClient.GetQueueReference(queueName);
            _Queue.CreateIfNotExists(new QueueRequestOptions()
            {
                RetryPolicy = config.GlobalRetryPolicy
            }, null);
        }
    }
}
