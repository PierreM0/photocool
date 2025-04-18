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
        ImageName.Content = imageName;
        using (var memoryStream = new MemoryStream(imageData))
        {
            ImagePreview.Source = new Bitmap(memoryStream);
        }
    }
}