using System;
using System.IO;
using Avalonia;
using Avalonia.Media.Imaging;

namespace photocool.Models;

public class ThumbnailPhotocool
{
    public static byte[] CreateThumbnailFromData(byte[] data)
    {
        Bitmap bitmap = new Bitmap(new MemoryStream(data));
        var originalWidth = bitmap.PixelSize.Width;
        var originalHeight = bitmap.PixelSize.Height;
        
        const int maxThumbSize = 125;
        
        double ratioX = (double)maxThumbSize / originalWidth;
        double ratioY = (double)maxThumbSize / originalHeight;
        double ratio = Math.Min(ratioX, ratioY);

        int scaledWidth = (int)(originalWidth * ratio);
        int scaledHeight = (int)(originalHeight * ratio);
        
        Bitmap resizedBitmap = bitmap.CreateScaledBitmap(new PixelSize(scaledWidth, scaledHeight));
        
        using var stream = new MemoryStream();
        resizedBitmap.Save(stream);
        return stream.ToArray();
    }
    
    public long Id { get; }
    public byte[] Data { get; }

    public ThumbnailPhotocool(long id, byte[] data)
    {
        Id = id;
        Data = data;
    }
}

// TODO afficher liste tags à gauche en hiérarchie avec TreeView
// TODO sélection multiple ?
// TODO importer plusieurs images en même temps