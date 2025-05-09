using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using photocool.DB;
using photocool.ViewModels;
using SkiaSharp;

namespace photocool.Views;

public partial class ImageCard : UserControl
{
    private long Id { get; }
    private int Index { get; set; }
    private Action RefreshAction { get; set; }
    private List<long> ImageIds { get; }
    
    public ImageCard(long id, int index, byte[] imageData, Action refreshAction, List<long> imageIds)
    { 
        InitializeComponent();

        Id = id;
        Index = index;
        Bitmap bitmap = new Bitmap(new MemoryStream(imageData));
        ImagePreview.Source = bitmap;

        DataContext = new ImageCardViewModel(id);
        
        RefreshAction = refreshAction;
        ImageIds = imageIds;
    }

    private void ImageCard_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            Window window = new ImageView(Id, Index, ImageIds);
            window.Show();
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

    private void ImageDelete_OnClick(object? sender, RoutedEventArgs e)
    {
        DatabaseManager.removeImage(Id);
        RefreshAction();
    }

    private void ImageModify_OnClick(object? sender, RoutedEventArgs e)
    {
        Window window = new ModifyImageWindow(Id);
        window.Show();
    }

    private async void ImageDownload_OnClick(object? sender, RoutedEventArgs e)
    {
        Window parentWindow = this.VisualRoot as Window;
        
        var dialog = new SaveFileDialog()
        {
            Title = "Téléchager une image",
            InitialFileName = "image.jpg",
            Filters = new()
            {
                new FileDialogFilter() { Name = "Image JPEG", Extensions = { "jpeg", "jpg" } }
            }
        };

        var filePath = await dialog.ShowAsync(parentWindow);
        if (string.IsNullOrEmpty(filePath))
            return;
        
        Bitmap image = new Bitmap(new MemoryStream(DatabaseManager.getImage(Id)));
        image.Save(filePath);
    }
}