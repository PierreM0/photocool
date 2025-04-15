using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using photocool.DB;
using photocool.ViewModels;

namespace photocool.Views;

public partial class NewTagWindow : Window
{
    public NewTagViewModel ViewModel { get; } = new();
    
    public NewTagWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    public string TagNameResult { get; private set; }
    public string TagParentResult { get; private set; }

    private void OK_Click(object? sender, RoutedEventArgs e)
    {
        TagNameResult = ViewModel.TagName;
        TagParentResult = ViewModel.TagParent;

        try
        {
            DatabaseManager.addTag(TagNameResult);
        }
        catch (MySqlException ex)
        {
            ViewModel.ErrorMessage = "Le tag '" + ViewModel.TagName + "' existe déjà !";
            return;
        }

        DatabaseManager.addParentToTag(TagNameResult, TagParentResult);
        
        Close();
    }
    
    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}