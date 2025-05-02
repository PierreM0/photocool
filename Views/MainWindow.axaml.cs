using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        
        RefreshImageGrid(null, null);
        Bar.NewFilter.Click += RefreshImageGrid;
        Bar.OnPillClick = new Action(() => RefreshImageGrid(null, null));
    }

    public void RefreshImageGrid(object? sender, RoutedEventArgs e)
    {
        bool allFilters = false;
        if (AllFiltersCheck.IsChecked.HasValue)
            allFilters = AllFiltersCheck.IsChecked.Value;
        
        ViewModel.HandleRefreshImageGrid(Bar.PillsList.List, ImageGrid, allFilters);
    }
}