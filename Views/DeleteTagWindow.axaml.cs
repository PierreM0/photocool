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
    private DeleteTagViewModel ViewModel { get; } = new();
    
    public DeleteTagWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void Delete_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleDelete();
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}