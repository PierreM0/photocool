using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using photocool.DB;

namespace photocool.Views;

public partial class ImageView : Window
{
    public ImageView(long id)
    {
        InitializeComponent();

        FullImage.Source = new Bitmap(new MemoryStream(DatabaseManager.getImage(id)));
    }
}