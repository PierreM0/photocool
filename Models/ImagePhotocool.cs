namespace photocool.Models;

public class ImagePhotocool
{
    public string Name { get; }
    public byte[] Data { get; }

    public ImagePhotocool(string name, byte[] data)
    {
        Name = name;
        Data = data;
    }
}

// TODO afficher liste tags à gauche en hiérarchie avec TreeView
// TODO sélection multiple ?
// TODO importer plusieurs images en même temps