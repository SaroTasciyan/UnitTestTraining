using System.Collections;
using System.Collections.Generic;

namespace Innova.Utilities.Test.Stubs
{
    public class ListStub<T> : IList<T>
    {
        private readonly List<T> list;

        public ListStub(List<T> list)
        {
            this.list = list;
        }

        public int Count => list.Count;

        public T this[int index]
        {
            get { return list[index]; }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsEnumerated { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            IsEnumerated = true;
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsReadOnly => false;

        #region Ignored

        public void Add(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }
        
        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}