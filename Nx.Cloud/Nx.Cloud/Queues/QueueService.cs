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
        private readonly ILogger _logger;
        private CloudQueue _queue;

        public QueueService(ILogFactory logFactory,
            ICloudConfiguration config, string queueName)
            : this(logFactory)
        {
            var queueClient = config.StorageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(queueName);
            _queue.CreateIfNotExists(new QueueRequestOptions()
            {
                RetryPolicy = config.GlobalRetryPolicy
            }, null);
        }

        private QueueService(ILogFactory logFactory)
        {
            _logger = logFactory.CreateLogger("QueueService");
        }

        public int Length
        {
            get
            {
                _queue.FetchAttributes();
                return _queue.ApproximateMessageCount ?? 0;
            }
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public void Delete()
        {
            _queue.Delete();
        }

        public T Dequeue()
        {
            T result;
            _logger.Debug("Attempting to dequeue item");

            var message = _queue.GetMessage();
            if (message != null)
            {
                result = SerializationHelper<T>.Deserialize(message.AsBytes);
                _queue.DeleteMessage(message);
                _logger.Debug("Item dequeued");
            }
            else
            {
                _logger.Debug("There was no item in the queue");
                result = null;
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Enqueue(T data)
        {
            Condition.Require<ArgumentException>(data != null);

            _logger.Debug("Enqueueing item");
            _queue.AddMessage(new CloudQueueMessage(SerializationHelper<T>.SerializeToByteArray(data)));
        }

        protected virtual void OnDisposing()
        {
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _queue = null;
                _logger.Dispose();
                OnDisposing();
            }
        }
    }
}