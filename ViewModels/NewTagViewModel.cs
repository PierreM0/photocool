using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Media;
using photocool.DB;

namespace photocool.ViewModels;

public class NewTagViewModel : ViewModel
{
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

    private ObservableCollection<string> _tags;
    public ObservableCollection<string> Tags
    {
        get => _tags;
        set { _tags = value; OnPropertyChanged(nameof(Tags)); }
    }

    private string _message;
    public string Message
    {
        get => _message;
        set { _message = value; OnPropertyChanged(nameof(Message)); }
    }

    private Brush _messageColor;
    public Brush MessageColor
    {
        get => _messageColor;
        set { _messageColor = value; OnPropertyChanged(nameof(MessageColor)); }
    }

    public NewTagViewModel()
    {
        _tagName = string.Empty;
        _tagParent = string.Empty;
        _tags = new();
        _message = string.Empty;
        _messageColor = new SolidColorBrush(Colors.Black);
        
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

    public void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}