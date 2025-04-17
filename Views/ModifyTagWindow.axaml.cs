using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using photocool.DB;
using photocool.ViewModels;

namespace photocool.Views;

public partial class ModifyTagWindow : Window
{
    private static Brush RED = new SolidColorBrush(Colors.Red);
    private static Brush GREEN = new SolidColorBrush(Colors.Green);
    
    private ModifyTagViewModel ViewModel = new();
    
    public ModifyTagWindow()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void Modify_Click(object? sender, RoutedEventArgs e)
    {
        string tagToModify = ViewModel.TagToModify;
        string newTagName = ViewModel.NewTagName;
        string newTagParent = ViewModel.NewTagParent;

        bool nameModified = false;

        if (string.IsNullOrWhiteSpace(tagToModify))
        {
            ViewModel.SetMessage("Le tag à modifier n'existe pas!", RED);
            return;
        }

        if (tagToModify == TagRepository.Root)
        {
            ViewModel.SetMessage("Vous ne pouvez pas modifier le tag racine!", RED);
        }

        if (DatabaseManager.getTagId(newTagName) != -1)
        {
            ViewModel.SetMessage("Le tag '" + newTagName + "' existe déjà!", RED);
            return;
        }

        if (string.IsNullOrWhiteSpace(newTagParent))
        {
            ViewModel.SetMessage("Le tag parent spécifié n'existe pas!", RED);
            return;
        }

        if (!string.IsNullOrWhiteSpace(newTagName))
        {
            DatabaseManager.modifyTag(tagToModify, newTagName);
            tagToModify = newTagName;
            nameModified = true;
            ViewModel.SetMessage("Le nom du tag a été modifié !", GREEN);
            TagRepository.Refresh();
        }

        if (!string.IsNullOrEmpty(newTagParent))
        {
            DatabaseManager.modifyTagParent(tagToModify, newTagParent);
            ViewModel.SetMessage("Le parent du tag a été modifié !", GREEN);
            if (nameModified)
            {
                ViewModel.SetMessage("Le nom et le parent du tag ont été modifiés !", GREEN);
            }
            TagRepository.Refresh();
        }
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}