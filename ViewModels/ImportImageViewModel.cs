using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using photocool.DB;
using photocool.Views;

namespace photocool.ViewModels;

public class ImportImageViewModel : ViewModel
{
    private ImportImageWindow _window;
    private string _imagePath;
    
    private string _imageName;
    public string ImageName
    {
        get => _imageName;
        set { _imageName = value; OnPropertyChanged(nameof(ImageName)); }
    }
    
    private string _message;
    public string Message
    {
        get => _message;
        set { _message = value; OnPropertyChanged(nameof(Message)); }
    }

    private Brush _messageColor;
    public Brush MessageColor
    {
        get => _messageColor;
        set { _messageColor = value; OnPropertyChanged(nameof(MessageColor)); }
    }
    
    public ImportImageViewModel(ImportImageWindow window)
    {
        TagRepository.Refresh();
        _window = window;
        _imagePath = string.Empty;
        _message = string.Empty;
    }

    private List<string> GetChosenTags()
    {
        List<string> tags = new();
        foreach (Pill pill in _window.Bar.PillsList.List)
        {
            tags.Add(pill.Text);
        }
        Console.WriteLine(tags.Count);
        return tags;
    }

    public async void HandleSelectImage()
    {
        OpenFileDialog dialog = new();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpeg", "jpg" } });
        
        string[]? result = await dialog.ShowAsync(_window);
        if (result != null && result.Length > 0)
        {
            string path = result[0];
            Bitmap bitmap = new Bitmap(path);
            _window.ImagePreview.Source = bitmap;
            _imagePath = path;
        }
    }

    public void HandleImport()
    {
        if (string.IsNullOrEmpty(_imagePath))
        {
            SetMessage("Veuillez sélectionner une image!", RED);
            return;
        }
        
        if (string.IsNullOrWhiteSpace(ImageName))
        {
            SetMessage("Veuillez renseigner un nom à l'image!", RED);
            return;
        }

        if (DatabaseManager.getImageId(ImageName) != -1)
        {
            SetMessage("Le nom d'image a déjà été utilisé!", RED);
            return;
        }
        
        List<string> tags = GetChosenTags();
        
        DatabaseManager.addImage(_imagePath, ImageName);

        foreach (string tag in tags)
        {
            DatabaseManager.addTagToImage(ImageName, tag);
        }
        
        SetMessage("L'image a été ajoutée avec succès!", GREEN);
        ImageName = string.Empty;
    }
    
    private void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}