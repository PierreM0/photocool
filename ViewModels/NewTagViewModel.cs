using System.Collections.ObjectModel;
using System.ComponentModel;
using photocool.DB;

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

    private ObservableCollection<string> _tags = new();
    public ObservableCollection<string> Tags
    {
        get => _tags;
        set { _tags = value; OnPropertyChanged(nameof(Tags)); }
    }

    public NewTagViewModel()
    {
        RefreshTags();
    }

    public void RefreshTags()
    {
        _tags.Clear();
        foreach (string tag in DatabaseManager.getAllTags())
        {
            _tags.Add(tag);
        }
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}