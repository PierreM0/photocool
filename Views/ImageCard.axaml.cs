using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace photocool.Views;

public partial class ImageCard : UserControl
{
    public ImageCard(string imageName, byte[] imageData)
    { 
        InitializeComponent();
        ImageName.Text = imageName;
        
        Bitmap bitmap = new Bitmap(new MemoryStream(imageData));
        var originalWidth = bitmap.PixelSize.Width;
        var originalHeight = bitmap.PixelSize.Height;
        
        const int maxThumbSize = 125;
        
        double ratioX = (double)maxThumbSize / originalWidth;
        double ratioY = (double)maxThumbSize / originalHeight;
        double ratio = Math.Min(ratioX, ratioY);

        int scaledWidth = (int)(originalWidth * ratio);
        int scaledHeight = (int)(originalHeight * ratio);
        
        var resizedBitmap = bitmap.CreateScaledBitmap(new PixelSize(scaledWidth, scaledHeight));

        ImagePreview.Source = resizedBitmap;
    }
}