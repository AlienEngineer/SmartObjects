using System;
using System.Collections.Generic;

namespace SmartObjects
{
    public class RecycleBin<T> : IRecycleBin<T>, IObjectFactory<T> where T : new()
    {
        private readonly int _maxCapacity;
        readonly Queue<T> _queue;

        public RecycleBin(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _queue = new Queue<T>();
        }
        
        public bool IsEmpty => _queue.Count == 0;

        public bool Contains(T item)
        {
            return _queue.Contains(item);
        }
        
        #region IRecycleBin<T>

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

        public void Purge()
        {
            _queue.Clear();
        }

        #endregion

        #region IObjectFactory<T>

        public T GetInstance()
        {
            return IsEmpty 
                ? new T() 
                : _queue.Dequeue();
        }

        #endregion

    }
}