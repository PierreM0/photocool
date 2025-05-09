using System;
using System.Collections.ObjectModel;
using photocool.DB;
using photocool.Models;

namespace photocool.ViewModels;

public class TagTreeViewViewModel : ViewModel
{
    public ObservableCollection<TagNode> TagNodes => TagRepository.TagNodes;

    public TagTreeViewViewModel()
    {
        
    }
    
    public void ExecuteDeparentTag(object? tag)
    {
        DatabaseManager.removeParentFromTag((tag as TagNode).Tag);
        Console.WriteLine((tag as TagNode).Tag);
        TagRepository.Refresh();
    }

    public void ExecuteDeleteTag(object? tag)
    {
        DatabaseManager.removeTag((tag as TagNode).Tag);
        TagRepository.Refresh();
    }
}