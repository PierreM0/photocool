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
        
        RefreshImageGrid(null, null);
        Bar.NewFilter.Click += RefreshImageGrid;
        Bar.OnPillClick = () => RefreshImageGrid(null, null);
    }

    public void RefreshImageGrid(object? sender, RoutedEventArgs e)
    {
        bool allFilters = false;
        if (AllFiltersCheck.IsChecked.HasValue)
            allFilters = AllFiltersCheck.IsChecked.Value;
        
        ViewModel.HandleRefreshImageGrid(Bar.PillsList.List, ImagePanel, allFilters);
    }

    private async void AddTagToParent(object? sender, RoutedEventArgs e)
    {
        Window window = new NewTagWindow((TagTreeView.SelectedItem as TagNode).Tag);
        Window? parentWindow = this.VisualRoot as Window;
        if (parentWindow == null)
            return;
        
        await window.ShowDialog(parentWindow);
    }

    private void DeleteTag(object? sender, RoutedEventArgs e)
    {
        Bar.TryRemoveFilter((TagTreeView.SelectedItem as TagNode).Tag);
        ViewModel.ExecuteDeleteTag(TagTreeView.SelectedItem);
    }

    private async void ModifyTag(object? sender, RoutedEventArgs e)
    {
        string tag = (TagTreeView.SelectedItem as TagNode).Tag;
        
        Window window = new ModifyTagWindow(tag);
        Window? parentWindow = this.VisualRoot as Window;
        if (parentWindow == null)
            return;
        
        await window.ShowDialog(parentWindow);

        TagRepository.Refresh();
        if (!TagRepository.Contains(tag))
        {
            Bar.TryRemoveFilter(tag);
        }
        RefreshImageGrid(null, null);
    }

    private void DeparentTag(object? sender, RoutedEventArgs e)
    {
        ViewModel.ExecuteDeparentTag(TagTreeView.SelectedItem);
        RefreshImageGrid(null, null);
    }

    private void TagNode_OnPointerPress(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            if (sender is Border border && border.Child is TextBlock textBlock)
            {
                Bar.AddFilter(textBlock.Text);
                RefreshImageGrid(null, null);
            }
        }
    }
}