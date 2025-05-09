using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using photocool.DB;

namespace photocool.Views;

public partial class ImageView : Window
{
    private int Index { get; set; }
    private List<long> ImageIds { get; set; }
    
    public ImageView(long id, int index, List<long> imageIds)
    {
        InitializeComponent();

        FullImage.Source = new Bitmap(new MemoryStream(DatabaseManager.getImage(id)));

        Index = index;
        ImageIds = imageIds;
        Counter.Content = (Index+1) + "/" + ImageIds.Count;
    }

    private void PreviousImage_OnClick(object? sender, RoutedEventArgs e)
    {
        ExecutePrevious();
    }

    private void NextImage_OnClick(object? sender, RoutedEventArgs e)
    {
        ExecuteNext();
    }

    private void ExecutePrevious()
    {
        if (Index > 0)
        {
            Index--;
            FullImage.Source = new Bitmap(new MemoryStream(DatabaseManager.getImage(ImageIds[Index])));
            Counter.Content = (Index+1) + "/" + ImageIds.Count;
        }
    }

    private void ExecuteNext()
    {
        if (Index < ImageIds.Count - 1)
        {
            Index++;
            FullImage.Source = new Bitmap(new MemoryStream(DatabaseManager.getImage(ImageIds[Index])));
            Counter.Content = (Index+1) + "/" + ImageIds.Count;
        }
    }

    private void Window_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space || e.Key == Key.Enter || e.Key == Key.Right || e.Key == Key.Up)
        {
            ExecuteNext();
        }

        if (e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Down)
        {
            ExecutePrevious();
        }
    }
}