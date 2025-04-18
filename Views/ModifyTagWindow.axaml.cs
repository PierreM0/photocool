using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using photocool.DB;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ModifyTagWindow : Window
{
    private ModifyTagViewModel ViewModel = new();
    
    public ModifyTagWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void Modify_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleModify();
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}