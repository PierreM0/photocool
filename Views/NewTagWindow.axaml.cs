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

        if (string.IsNullOrWhiteSpace(tagParent))
        {
            ViewModel.SetMessage("Le tag parent spécifié n'existe pas!", RED);
            return;
        }

        if (tagName == tagParent)
        {
            ViewModel.SetMessage("Un tag ne peut pas être parent de lui-même!", RED);
            return;
        }

        try
        {
            DatabaseManager.addTagWithParent(tagName, tagParent);
        }
        catch (MySqlException ex)
        {
            if (ex.Number == DatabaseManager.DUPLICATE_ENTRY)
            {
                ViewModel.SetMessage("Le tag '" + tagName + "' existe déjà !", RED);
            }
            else
            {
                ViewModel.SetMessage("Une erreur est survenue.", RED);
            }

            return;
        }
        
        ViewModel.SetMessage("Le tag '" + tagName + "' a été ajouté!", GREEN);
        TagRepository.Refresh();
    }
    
    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}