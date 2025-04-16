using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using photocool.DB;
using photocool.ViewModels;

namespace photocool.Views;

public partial class DeleteTagWindow : Window
{
    private static Brush RED = new SolidColorBrush(Colors.Red);
    private static Brush GREEN = new SolidColorBrush(Colors.Green);

    private DeleteTagViewModel ViewModel { get; } = new();
    
    public DeleteTagWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void Delete_Click(object? sender, RoutedEventArgs e)
    {
        string tagName = ViewModel.TagName;
        DatabaseManager.removeTag(tagName);
        ViewModel.SetMessage("Le tag '" + tagName + "' a été supprimé!", GREEN);
        TagRepository.Refresh();
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}