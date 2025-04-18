using System;
using Avalonia;
using Avalonia.Controls;
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
        RefreshImageGrid();
    }

    public void RefreshImageGrid()
    {
        ViewModel.HandleRefreshImageGrid(Bar.PillsList.List, ImageGrid);
    }
}