using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ViewModel.MVVM
{
    public class SuppressNotifyObservableCollection<T> : ObservableCollection<T>
    {
        public bool SuppressNotify { get; set; } = false;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressNotify)
                base.OnCollectionChanged(e);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!SuppressNotify)
                base.OnPropertyChanged(e);
        }
    }
}

