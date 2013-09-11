using System;

namespace Nx.Events
{
    public class EventArgs<T> : EventArgs
    {
        private readonly T _value;
        public T Value
        {
            get { return _value; }
        }

        public EventArgs(T value)
        {
            _value = value;
        }
    }
}
