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
    private void TagButton_Handler(object? sender, RoutedEventArgs e)
    {
        try
        {
            //DatabaseManager.addImage("C:\\Users\\Adam\\Pictures\\images_coolmdr\\RDT_20240424_0206313735683715627756656.jpg",
            //    "test", "usopp");
            List<long> tagIds = new List<long>();
            tagIds.Add(11L);
            tagIds.Add(1L);
            foreach (var VARIABLE in DatabaseManager.getImages(tagIds))
            {
                Console.WriteLine(VARIABLE.Key);
            }
            
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1062) // 1062 = Duplicate entry
            {
                Console.WriteLine("Doublon détecté !");
            }
            else
            {
                Console.WriteLine("Erreur MySQL : " + ex.Message);
            }
        }

    }
    /**TODO:
     * récupérer images avec n tags (ET) OU is done
     */
    private void ImageButton_Handler(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}