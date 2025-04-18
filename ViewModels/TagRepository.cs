using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using photocool.DB;

namespace photocool.ViewModels;

public class TagRepository
{
    public static ObservableCollection<string> Tags { get; } = new();

    public static bool Contains(string tag)
    {
        return Tags.Contains(tag);
    }

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