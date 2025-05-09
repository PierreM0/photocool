using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using photocool.DB;
using photocool.Models;

namespace photocool.ViewModels;

public class TagRepository
{
    public static ObservableCollection<string> Tags { get; } = new();
    
    public static ObservableCollection<TagNode> TagNodes { get; } = new();

    public static bool Contains(string tag)
    {
        return Tags.Contains(tag);
    }

    public static void Refresh()
    {
        List<string> newTags = DatabaseManager.getAllTags();
        
        Tags.Clear();
        Tags.Add(string.Empty);
        foreach (string tag in newTags)
        {
            Tags.Add(tag);
        }

        Dictionary<long, long> tagFamily = DatabaseManager.getTagFamily();
        Dictionary<long, string> tagNames = DatabaseManager.getAllTagsAndIds();

        Dictionary<long, TagNode> nodeMap = new();

        foreach (var (id, tag) in tagNames)
        {
            nodeMap[id] = new TagNode(id, tag);
        }

        foreach (var (childId, parentId) in tagFamily)
        {
            if (nodeMap.TryGetValue(parentId, out var parent) && nodeMap.TryGetValue(childId, out var child))
            {
                parent.Children.Add(child);
            }
        }

        TagNodes.Clear();
        HashSet<long> childIds = new(tagFamily.Keys);
        foreach (var (id, node) in nodeMap)
        {
            if (!tagFamily.ContainsKey(id))
            {
                TagNodes.Add(node);
            }
        }
    }
}