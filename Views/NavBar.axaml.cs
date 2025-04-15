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
    
    /**
     * Tags done.
     */
    private async void TagButton_Handler(object? sender, RoutedEventArgs e)
    {
        Window? parentWindow = this.VisualRoot as Window;
        
        var window = new NewTagWindow();
        await window.ShowDialog(parentWindow);
    }
    /**TODO:
     * récupérer images avec n tags (ET) OU is done
     */
    private void ImageButton_Handler(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}