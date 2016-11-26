using System.Collections;
using System.Collections.Generic;

namespace SmartObjects
{
    public class RecycleCollection<T> : ICollection<T>
    {
        private readonly IRecycleBin<T> _recycleBin;
        private readonly ICollection<T> _underlyingCollection;

        public RecycleCollection(IRecycleBin<T> recycleBin, ICollection<T> underlyingCollection)
        {
            _recycleBin = recycleBin;
            _underlyingCollection = underlyingCollection;
        }

        public void Add(T item)
        {
            _underlyingCollection.Add(item);
        }

        public void Clear()
        {
            _recycleBin.Recycle(_underlyingCollection);
            _underlyingCollection.Clear();
        }

        public bool Contains(T item)
        {
            return _underlyingCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _underlyingCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            _recycleBin.Recycle(item);
            return _underlyingCollection.Remove(item);
        }

        public int Count => _underlyingCollection.Count;

        public bool IsReadOnly => _underlyingCollection.IsReadOnly;

        public IEnumerator<T> GetEnumerator()
        {
            return _underlyingCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}