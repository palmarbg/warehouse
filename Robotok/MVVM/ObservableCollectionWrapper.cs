using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotok.MVVM
{
    public class ObservableCollectionWrapper<T> : ICollection<T>, INotifyCollectionChanged
    {
        private ICollection<T> _collection = null!;
        public ICollection<T> Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                _collection = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        public ObservableCollectionWrapper(ICollection<T> Collection)
        {
            this.Collection = Collection;
        }

        public void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #region ICollection<T>

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;


        public void Add(T item)
        {
            throw new NotImplementedException();

        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
        #endregion
    }
}
