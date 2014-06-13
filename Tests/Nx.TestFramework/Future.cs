using System;
using System.Threading;

namespace Nx
{
    /// <summary>
    /// A future object that supports both callbacks and asynchronous waits once a future value becomes available.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Future<T> :
    IAsyncResult
    {
        private readonly AsyncCallback _callback;
        private readonly ManualResetEvent _event;
        private readonly object _state;
        private volatile bool _completed;

        public Future()
            : this(NullCallback, 0)
        {
        }

        public Future(AsyncCallback callback, object state)
        {
            Condition.ArgumentNotNull(callback, "callback");

            _callback = callback;
            _state = state;

            _event = new ManualResetEvent(false);
        }

        public T Value { get; private set; }

        public bool IsCompleted
        {
            get { return _completed; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return _event; }
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public void Complete(T message)
        {
            if (_completed)
            {
                throw new InvalidOperationException(string.Format("A Future cannot be completed twice, value = {0}, passed = {1}", Value, message));
            }

            Value = message;

            _completed = true;

            _event.Set();

            _callback(this);
        }

        public bool WaitUntilCompleted(TimeSpan timeout)
        {
            return _event.WaitOne(timeout);
        }

        public bool WaitUntilCompleted(int timeout)
        {
            return _event.WaitOne(timeout);
        }

        ~Future()
        {
            _event.Close();
        }

        private static void NullCallback(object state)
        {
        }
    }
}