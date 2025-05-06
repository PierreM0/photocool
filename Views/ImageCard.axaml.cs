using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace photocool.Views;

public partial class ImageCard : UserControl
{
    public ImageCard(byte[] imageData)
    { 
        InitializeComponent();

        Bitmap bitmap = new Bitmap(new MemoryStream(imageData));
        ImagePreview.Source = bitmap;
    }
}