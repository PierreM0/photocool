using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using photocool.DB;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ModifyImageWindow : Window
{
    private ModifyImageViewModel ViewModel { get; set; }
    
    public ModifyImageWindow(long id)
    {
        InitializeComponent();
        
        ViewModel = new ModifyImageViewModel(id);
        DataContext = ViewModel;

        List<string> tags = DatabaseManager.GetImageTags(id);
        foreach (string tag in tags)
        {
            Bar.AddFilter(tag);
        }

        TagTree.Bar = Bar;
        TagTree.Bar.OnPillClick = () => { };
        TagTree.Refresh = () => { };
    }
    
    private void Modify_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.ExecuteModify(Bar.PillsList.List);
        Close();
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}