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

    public string TagNameResult { get; private set; }
    public string TagParentResult { get; private set; }

    private void Add_Click(object? sender, RoutedEventArgs e)
    {
        TagNameResult = ViewModel.TagName;
        TagParentResult = ViewModel.TagParent;

        if (TagNameResult == string.Empty || TagParentResult == string.Empty)
        {
            ViewModel.SetMessage("Veuillez renseignez tous les champs!", RED);
            return;
        }

        if (TagNameResult == TagParentResult)
        {
            ViewModel.SetMessage("Un tag ne peut pas être parent de lui-même!", RED);
            return;
        }

        try
        {
            DatabaseManager.addTagWithParent(TagNameResult, TagParentResult);
        }
        catch (MySqlException ex)
        {
            if (ex.Number == DatabaseManager.DUPLICATE_ENTRY)
            {
                ViewModel.SetMessage("Le tag '" + TagNameResult + "' existe déjà !", RED);
            }
            else
            {
                ViewModel.SetMessage("Une erreur est survenue.", RED);
            }

            return;
        }
        
        ViewModel.SetMessage("Le tag '" + TagNameResult + "' a été ajouté!", GREEN);
        ViewModel.RefreshTags();
    }
    
    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}