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

        if (string.IsNullOrEmpty(tagToModify))
        {
            ViewModel.SetMessage("Veuillez renseignez le tag à modifier !", RED);
            return;
        }

        if (string.IsNullOrEmpty(newTagName) && string.IsNullOrEmpty(newTagParent))
        {
            ViewModel.SetMessage("Veuillez renseignez une modification à effectuer !", RED);
            return;
        }

        if (!string.IsNullOrEmpty(newTagName))
        {
            DatabaseManager.modifyTag(tagToModify, newTagName);
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