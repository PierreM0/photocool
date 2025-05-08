using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ModifyImageWindow : Window
{
    public ModifyImageWindow()
    {
        InitializeComponent();
        
        DataContext = new ModifyImageViewModel();
    }
    
    private void Modify_Click(object? sender, RoutedEventArgs e)
    {
        return;
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}