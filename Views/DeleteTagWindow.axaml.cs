using System;
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
        
        if (string.IsNullOrWhiteSpace(tagName))
        {
            ViewModel.SetMessage("Veuillez spécifier un tag à supprimer!", RED);
            return;
        }

        if (tagName == TagRepository.Root)
        {
            ViewModel.SetMessage("Vous ne pouvez pas supprimer le tag racine!", RED);
            return;
        }

        if (DatabaseManager.getTagId(tagName) == -1)
        {
            ViewModel.SetMessage("Le tag '" + tagName + "' n'existe pas!", RED);
            return;
        }
        
        DatabaseManager.removeTag(tagName);
        
        ViewModel.SetMessage("Le tag '" + tagName + "' a été supprimé!", GREEN);
        TagRepository.Refresh();
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}