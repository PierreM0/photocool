using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MySql.Data.MySqlClient;
using photocool.DB;
using photocool.ViewModels;

namespace photocool.Views;

public partial class NewTagWindow : Window
{
    private static Brush RED = new SolidColorBrush(Colors.Red);
    private static Brush GREEN = new SolidColorBrush(Colors.Green);
    public NewTagViewModel ViewModel { get; } = new();
    
    public NewTagWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
    
    private void Add_Click(object? sender, RoutedEventArgs e)
    {
        string tagName = ViewModel.TagName;
        string tagParent = ViewModel.TagParent;

        if (string.IsNullOrWhiteSpace(tagName))
        {
            ViewModel.SetMessage("Veuillez spécifier le nom du nouveau tag!", RED);
            return;
        }

        if (DatabaseManager.getTagId(tagName) != -1)
        {
            ViewModel.SetMessage("Le tag '" + tagName + "' existe déjà!", RED);
            return;
        }

        if (DatabaseManager.getTagId(tagParent) == -1)
        {
            ViewModel.SetMessage("Le tag '" + tagParent + "' n'existe pas!", RED);
            return;
        }

        if (string.IsNullOrWhiteSpace(tagParent))
        {
            DatabaseManager.addTag(tagName);
        }
        else
        {
            DatabaseManager.addTagWithParent(tagName, tagParent);
        }
        
        ViewModel.SetMessage("Le tag '" + tagName + "' a été ajouté!", GREEN);
        TagRepository.Refresh();
    }
    
    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}