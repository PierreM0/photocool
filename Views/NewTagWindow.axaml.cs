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

        try
        {
            DatabaseManager.addTag(TagNameResult);
        }
        catch (MySqlException ex)
        {
            ViewModel.SetMessage("Le tag '" + ViewModel.TagName + "' existe déjà !", new SolidColorBrush(Colors.Red));
            return;
        }

        // TODO check if parent exists + rollback?
        DatabaseManager.addParentToTag(TagNameResult, TagParentResult);
    }
    
    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}