using System.Collections.ObjectModel;
using photocool.DB;

namespace photocool.ViewModels;

public class TagRepository
{
    public static ObservableCollection<string> Tags { get; } = new();

    public static void Refresh()
    {
        Tags.Clear();
        Tags.Add(string.Empty);
        foreach (string tag in DatabaseManager.getAllTags())
        {
            Tags.Add(tag);
        }
    }
}