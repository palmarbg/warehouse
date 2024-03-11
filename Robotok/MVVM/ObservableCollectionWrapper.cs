using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Robotok.MVVM
{
    public class ObservableCollectionWrapper<T> : IReadOnlyList<T>, INotifyCollectionChanged
    {
        private IList<T> _collection = null!;
        public IList<T> Collection
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


        public ObservableCollectionWrapper(IList<T> Collection)
        {
            this.Collection = Collection;
        }

        public void OnCollectionChanged()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
            
        }

        #region IReadOnlyList<T>

        public int Count => _collection.Count;

        public bool IsReadOnly => true;

        public T this[int index] => _collection[index];


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
