using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfPaint.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var invocationList = PropertyChanged?.GetInvocationList();
            if (invocationList is null)
                return;
            foreach(var action in invocationList)
            {
                if (action.Target is DispatcherObject target)
                    target.Dispatcher.BeginInvoke(action, this, new PropertyChangedEventArgs(propertyName));
                else
                    action.DynamicInvoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected virtual bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if(Equals(oldValue, newValue)) return false;
            oldValue = newValue;
            OnPropertyChanged(propertyName); 
            return true;
        }
    }
}
