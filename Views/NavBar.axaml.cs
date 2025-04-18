using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using photocool.DB;

namespace photocool.Views;

public partial class NavBar : UserControl
{
    public NavBar()
    {
        InitializeComponent();
    }

    private async void OpenWindowAndShowDialog(Window window)
    {
        Window? parentWindow = this.VisualRoot as Window;
        if (parentWindow == null)
            return;
        
        await window.ShowDialog(parentWindow);
    }
    
    private void Add_Tag_Click(object? sender, RoutedEventArgs e)
    {
        OpenWindowAndShowDialog(new NewTagWindow());
    }
    
    private void Modify_Tag_Click(object? sender, RoutedEventArgs e)
    {
        OpenWindowAndShowDialog(new ModifyTagWindow());
    }
    
    private void Delete_Tag_Click(object? sender, RoutedEventArgs e)
    {
        OpenWindowAndShowDialog(new DeleteTagWindow());
    }
    
    private void Import_Image_Click(object? sender, RoutedEventArgs e)
    {
        OpenWindowAndShowDialog(new ImportImageWindow());
    }
}