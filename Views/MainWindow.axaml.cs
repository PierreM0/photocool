using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using photocool.Models;
using photocool.ViewModels;

namespace photocool.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel;
    
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new();
        DataContext = ViewModel;
        
        RefreshImages();
        Bar.NewFilter.Click += RefreshImageGrid;
        Bar.OnPillClick = () => RefreshImages();

        TagTree.Bar = Bar;
        TagTree.Refresh = () => RefreshImages();
    }

    public void RefreshImageGrid(object? sender, RoutedEventArgs e)
    {
        RefreshImages();
    }

    public void RefreshImages()
    {
        bool allFilters = false;
        if (AllFiltersCheck.IsChecked.HasValue)
            allFilters = AllFiltersCheck.IsChecked.Value;
        
        ViewModel.HandleRefreshImageGrid(Bar.PillsList.List, ImagePanel, allFilters);
    }
}