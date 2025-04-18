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
    
    private void Add_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleAdd();
    }
    
    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}