using System.Collections.ObjectModel;
using Avalonia.Media;

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

    public ObservableCollection<string> Tags { get; }

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
        _message = string.Empty;
        _messageColor = new SolidColorBrush(Colors.Black);
        Tags = TagRepository.Tags;
        
        TagRepository.Refresh();
    }

    public void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}