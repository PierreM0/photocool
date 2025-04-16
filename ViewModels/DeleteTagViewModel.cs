using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Media;
using photocool.DB;
using ZstdSharp.Unsafe;

namespace photocool.ViewModels;

public class DeleteTagViewModel : ViewModel
{
    private string _tagName;
    public string TagName
    {
        get => _tagName;
        set { _tagName = value;
            OnPropertyChanged(nameof(TagName));
        }
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

    public DeleteTagViewModel()
    {
        _tagName = string.Empty;
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