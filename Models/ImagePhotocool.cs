namespace photocool.Models;

public class ImagePhotocool
{
    public long Id { get; }
    public byte[] Data { get; }

    public ImagePhotocool(long id, byte[] data)
    {
        Id = id;
        Data = data;
    }
}

// TODO afficher liste tags à gauche en hiérarchie avec TreeView
// TODO sélection multiple ?
// TODO importer plusieurs images en même temps