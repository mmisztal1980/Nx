using System;

namespace Nx.Cloud.Queues
{
    public interface IQueueService<T> : IDisposable
        where T : class, new()
    {
        int Length { get; }

        void Clear();

        void Delete();

        T Dequeue();

        void Enqueue(T data);
    }
}