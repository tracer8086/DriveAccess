using System;
using System.Collections.Generic;
using System.Text;

namespace DriveAccessors
{
    public interface IIndexedStorage<T> : IEnumerable<T>
    {
        T this[int index] { get; }

        void Add(T item);
    }
}
