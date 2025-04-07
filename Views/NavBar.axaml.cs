using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
        DatabaseManager.addTagWithParent("testAddTagWithParent","test");
    }
    /**
     * TODO: ajouter image bdd
     * ajouter une image avec une image, un nom et au moins 1 tag
     * modifier les tags d'une image
     * récupérer les images
     * récupérer images avec n tags (OU et ET)
     * supprimer image
     */
    private void ImageButton_Handler(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}