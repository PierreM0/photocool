using System.Collections.ObjectModel;
using Avalonia.Media;
using photocool.DB;

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

    public void HandleModify()
    {
        string tagToModify = TagToModify.Trim();
        string newTagName = NewTagName.Trim();
        string newTagParent = NewTagParent.Trim();
        
        bool nameModified = false;

        if (string.IsNullOrWhiteSpace(tagToModify))
        {
            SetMessage("Veuillez renseigner le tag à modifier!", RED);
            return;
        }
        
        if (DatabaseManager.getTagId(tagToModify) == -1)
        {
            SetMessage("Le tag '" + tagToModify + "' n'existe pas!", RED);
            return;
        }

        if (string.IsNullOrWhiteSpace(newTagName) && string.IsNullOrWhiteSpace(newTagParent))
        {
            SetMessage("Veuillez renseigner au moins une modification à effectuer!", RED);
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(newTagName) && DatabaseManager.getTagId(newTagName) != -1)
        {
            SetMessage("Le tag '" + newTagName + "' existe déjà!", RED);
            return;
        }

        if (!string.IsNullOrWhiteSpace(newTagParent) && DatabaseManager.getTagId(newTagParent) == -1)
        {
            SetMessage("Le tag '" + newTagParent + "' n'existe pas!", RED);
            return;
        }

        if (tagToModify == newTagParent)
        {
            SetMessage("Le tag à modifier et le nouveau parent ne peuvent pas avoir le même nom!", RED);
            return;
        }

        if (newTagName == newTagParent)
        {
            SetMessage("Le nouveau nom du tag et le nouveau parent ne peuvent pas avoir le même nom!", RED);
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(newTagName))
        {
            DatabaseManager.modifyTag(tagToModify, newTagName);
            tagToModify = newTagName;
            
            SetMessage("Le nom du tag a été modifié!", GREEN);
            nameModified = true;
        }

        if (!string.IsNullOrWhiteSpace(newTagParent))
        {
            DatabaseManager.modifyTagParent(tagToModify, newTagParent);
            
            SetMessage("Le parent du tag a été modifié!", GREEN);
            if (nameModified)
            {
                SetMessage("Le nom et le parent du tag ont été modifiés!", GREEN);
            }
        }
    }

    private void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}