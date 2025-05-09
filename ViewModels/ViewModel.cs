using System.ComponentModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace photocool.ViewModels;

public class ViewModel : INotifyPropertyChanged
{
    protected static Brush RED = new SolidColorBrush(Colors.Red);
    protected static Brush GREEN = new SolidColorBrush(Colors.Green);
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
