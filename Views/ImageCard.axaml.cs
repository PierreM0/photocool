using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ImageCard : UserControl
{
    private long Id { get; }
    
    public ImageCard(long id, byte[] imageData)
    { 
        InitializeComponent();

        Id = id;
        Bitmap bitmap = new Bitmap(new MemoryStream(imageData));
        ImagePreview.Source = bitmap;

        DataContext = new ImageCardViewModel(id);
    }

    private void ImageCard_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            Window window = new ImageView(Id);
            window.Show();
        }

        if (e.GetCurrentPoint(null).Properties.IsRightButtonPressed)
        {
            Console.WriteLine(Id);
        }
    }

    private void ImageCard_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        ImageBorder.Background = Brushes.PaleTurquoise;
    }

    private void ImageCard_OnPointerExited(object? sender, PointerEventArgs e)
    {
        ImageBorder.Background = Brushes.Azure;
    }
}