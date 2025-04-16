using System.Collections.ObjectModel;
using Avalonia.Media;

namespace photocool.ViewModels;

public class ModifyTagViewModel : ViewModel
{
    private string _tagToModify;
    public string TagToModify
    {
        get => _tagToModify;
        set { _tagToModify = value;
            OnPropertyChanged(nameof(TagToModify));
        }
    }
    
    private string _newTagName;
    public string NewTagName
    {
        get => _newTagName;
        set { _newTagName = value; OnPropertyChanged(nameof(NewTagName)); }
    }

    private string _newTagParent;
    public string NewTagParent
    {
        get => _newTagParent;
        set { _newTagParent = value; OnPropertyChanged(nameof(NewTagParent)); }
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

    public ModifyTagViewModel()
    {
        _tagToModify = string.Empty;
        _newTagName = string.Empty;
        _newTagParent = string.Empty;
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