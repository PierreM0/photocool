using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using photocool.Models;
using photocool.ViewModels;

namespace photocool.Views;

public partial class TagTreeView : UserControl
{
    public SearchBar Bar { get; set; }
    public Action Refresh { get; set; }
    private TagTreeViewViewModel ViewModel { get; set; }
    
    public TagTreeView()
    {
        InitializeComponent();
        ViewModel = new TagTreeViewViewModel();
        DataContext = ViewModel;
    }
    
    private async void AddTagToParent(object? sender, RoutedEventArgs e)
    {
        Window window = new NewTagWindow((TagTree.SelectedItem as TagNode).Tag);
        Window? parentWindow = this.VisualRoot as Window;
        if (parentWindow == null)
            return;
        
        await window.ShowDialog(parentWindow);
    }

    private void DeleteTag(object? sender, RoutedEventArgs e)
    {
        Bar.TryRemoveFilter((TagTree.SelectedItem as TagNode).Tag);
        ViewModel.ExecuteDeleteTag(TagTree.SelectedItem);
    }

    private async void ModifyTag(object? sender, RoutedEventArgs e)
    {
        string tag = (TagTree.SelectedItem as TagNode).Tag;
        
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
        Refresh();
    }

    private void DeparentTag(object? sender, RoutedEventArgs e)
    {
        ViewModel.ExecuteDeparentTag(TagTree.SelectedItem);
        Refresh();
    }

    private void TagNode_OnPointerPress(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            if (sender is Border border && border.Child is TextBlock textBlock)
            {
                Bar.AddFilter(textBlock.Text);
                Refresh();
            }
        }
    }
}