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
            ViewModel.SetMessage("Veuillez renseigner le tag à modifier!", RED);
            return;
        }
        
        if (DatabaseManager.getTagId(tagToModify) == -1)
        {
            ViewModel.SetMessage("Le tag '" + tagToModify + "' n'existe pas!", RED);
            return;
        }

        if (string.IsNullOrWhiteSpace(newTagName) && string.IsNullOrWhiteSpace(newTagParent))
        {
            ViewModel.SetMessage("Veuillez renseigner au moins une modification à effectuer!", RED);
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(newTagName) && DatabaseManager.getTagId(newTagName) != -1)
        {
            ViewModel.SetMessage("Le tag '" + newTagName + "' existe déjà!", RED);
            return;
        }

        if (!string.IsNullOrWhiteSpace(newTagParent) && DatabaseManager.getTagId(newTagParent) == -1)
        {
            ViewModel.SetMessage("Le tag '" + newTagParent + "' n'existe pas!", RED);
            return;
        }

        if (tagToModify == newTagParent)
        {
            ViewModel.SetMessage("Le tag à modifier et le nouveau parent ne peuvent pas avoir le même nom!", RED);
            return;
        }

        if (newTagName == newTagParent)
        {
            ViewModel.SetMessage("Le nouveau nom du tag et le nouveau parent ne peuvent pas avoir le même nom!", RED);
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(newTagName))
        {
            DatabaseManager.modifyTag(tagToModify, newTagName);
            tagToModify = newTagName;
            
            ViewModel.SetMessage("Le nom du tag a été modifié!", GREEN);
            nameModified = true;
        }

        if (!string.IsNullOrWhiteSpace(newTagParent))
        {
            DatabaseManager.modifyTagParent(tagToModify, newTagParent);
            
            ViewModel.SetMessage("Le parent du tag a été modifié!", GREEN);
            if (nameModified)
            {
                ViewModel.SetMessage("Le nom et le parent du tag ont été modifiés!", GREEN);
            }
        }
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}