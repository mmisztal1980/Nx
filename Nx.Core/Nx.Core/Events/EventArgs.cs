using System;

namespace Nx.Events
{
    public class EventArgs<T> : EventArgs
    {
        private readonly T value;
        public T Value
        {
            get { return this.value; }
        }

        public EventArgs(T value)
        {
            this.value = value;
        }
    }
}
