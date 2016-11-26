using System.Collections.Generic;

namespace SmartObjects
{
    public class RecycleBin<T> : IRecycleBin<T>
    {
        private readonly int _maxCapacity;
        readonly Queue<T> _queue;

        public bool IsEmpty => _queue.Count == 0;

        public RecycleBin(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _queue = new Queue<T>();
        }

        public void Recycle(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Recycle(item);
            }
        }

        public void Recycle(T item)
        {
            if (_maxCapacity > _queue.Count)
            {
                _queue.Enqueue(item);
            }
        }

        public bool Contains(T item)
        {
            return _queue.Contains(item);
        }

        public void Purge()
        {
            _queue.Clear();
        }
    }
}