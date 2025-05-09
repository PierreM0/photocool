using System;
using System.Reflection.Metadata.Ecma335;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ImportImageWindow : Window
{
    private ImportImageViewModel ViewModel = new();
    
    public ImportImageWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;

        TagTree.Bar = Bar;
        TagTree.Refresh = () => { };

        TagTree.Bar.OnPillClick = () => { };
    }

    private void Select_Image_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleSelectImage(this.VisualRoot as Window);
    }

    private void Import_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.HandleImport(Bar.PillsList.List);
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void RemoveImages_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.RemoveImages();
    }
}