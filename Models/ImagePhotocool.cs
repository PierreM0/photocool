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