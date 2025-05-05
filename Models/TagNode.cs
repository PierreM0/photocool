using System.Collections.ObjectModel;

namespace photocool.Models;

public class TagNode
{
    public ObservableCollection<TagNode> Children { get; }
    public long Id { get; }
    public string Tag { get; }

    public TagNode(long id, string tag)
    {
        Id = id;
        Tag = tag;
        Children = new();
    }
}