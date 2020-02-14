using System;
using System.ComponentModel;

namespace suit.models
{
    public class Task : INotifyPropertyChanged
    {
        public int Count { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
    
}
