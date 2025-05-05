using System.Collections.ObjectModel;
using Avalonia.Media;
using Mysqlx.Datatypes;
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

    public NewTagViewModel(string parent = "")
    {
        _tagName = string.Empty;
        _tagParent = parent;
        _message = string.Empty;
        _messageColor = new SolidColorBrush(Colors.Black);
        Tags = TagRepository.Tags;
        
        TagRepository.Refresh();
    }

    public void HandleAdd()
    {
        string tagName = TagName.Trim();
        string tagParent = TagParent.Trim();

        if (string.IsNullOrWhiteSpace(tagName))
        {
            SetMessage("Veuillez spécifier le nom du nouveau tag!", RED);
            return;
        }

        if (DatabaseManager.getTagId(tagName) != -1)
        {
            SetMessage("Le tag '" + tagName + "' existe déjà!", RED);
            return;
        }
        
        if (string.IsNullOrWhiteSpace(tagParent))
        {
            DatabaseManager.addTag(tagName);
        }
        
        if (!string.IsNullOrWhiteSpace(tagParent))
        {
            if (DatabaseManager.getTagId(tagParent) == -1)
            {
                SetMessage("Le tag '" + tagParent + "' n'existe pas!", RED);
                return;
            }
            
            DatabaseManager.addTagWithParent(tagName, tagParent);
        }
        
        SetMessage("Le tag '" + tagName + "' a été ajouté!", GREEN);
        TagRepository.Refresh();
    }

    private void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}