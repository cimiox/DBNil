using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace БДНИЛ_Учёт__деятельности_лабаратории.addition
{
    public abstract class Notifier : INotifyPropertyChanged
    {
        //метод который обрабатывает событие INotifyPropertyChanged, уведомление об изменении
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
