using System.ComponentModel;

namespace photocool.ViewModels;

public class NewTagViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _tagName;
    public string TagName
    {
        get => _tagName;
        set { _tagName = value;
            OnPropertyChanged(nameof(TagName));
        }
    }

    private string _tagParent;
    public string TagParent
    {
        get => _tagParent;
        set { _tagParent = value; OnPropertyChanged(nameof(TagParent)); }
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}