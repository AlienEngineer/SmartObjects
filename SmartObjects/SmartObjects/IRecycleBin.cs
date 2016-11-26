using System.Collections.Generic;

namespace SmartObjects
{
    public interface IRecycleBin<in T>
    {
        void Recycle(IEnumerable<T> items);
        void Recycle(T item);
    }
}
