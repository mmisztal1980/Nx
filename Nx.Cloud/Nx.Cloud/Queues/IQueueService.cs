using System;

namespace Nx.Cloud.Queues
{
    public interface IQueueService<T> : IDisposable
        where T : class, new()
    {
        int Length { get; }
        void Enqueue(T data);
        T Dequeue();
        void Clear();
        void Delete();
    }
}
