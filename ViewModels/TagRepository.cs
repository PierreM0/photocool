using System.Collections.ObjectModel;
using photocool.DB;

namespace photocool.ViewModels;

public class TagRepository
{
    public static ObservableCollection<string> Tags { get; set; } = new ObservableCollection<string>();

    public static void Refresh()
    {
        Tags.Clear();
        foreach (string tag in DatabaseManager.getAllTags())
        {
            Tags.Add(tag);
        }
    }
}