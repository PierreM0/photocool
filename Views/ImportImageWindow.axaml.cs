using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ImportImageWindow : Window
{
    private ImportImageViewModel ViewModel;
    
    public ImportImageWindow()
    {
        InitializeComponent();
        ViewModel = new(this);
        DataContext = ViewModel;
    }

    private void Select_Image_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleSelectImage();
    }

    private void Import_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleImport();
    }
}