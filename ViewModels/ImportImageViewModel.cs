using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using photocool.DB;
using photocool.Views;

namespace photocool.ViewModels;

public class ImportImageViewModel : ViewModel
{
    private string _imagePath;
    
    private Bitmap _imageSource;
    public Bitmap ImageSource
    {
        get => _imageSource;
        set { _imageSource = value; OnPropertyChanged(nameof(ImageSource)); }
    }
    
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
    
    public ImportImageViewModel()
    {
        TagRepository.Refresh();
        _imageSource = null;
        _imagePath = string.Empty;
        _message = string.Empty;
    }

    public async void HandleSelectImage(Window parentWindow)
    {
        OpenFileDialog dialog = new();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpeg", "jpg" } });
        
        string[]? result = await dialog.ShowAsync(parentWindow);
        if (result != null && result.Length > 0)
        {
            string path = result[0];
            Bitmap bitmap = new Bitmap(path);
            ImageSource = bitmap;
            _imagePath = path;
        }
    }

    public void HandleImport(List<Pill> pills)
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
        
        DatabaseManager.addImage(_imagePath, ImageName);

        foreach (Pill pill in pills)
        {
            DatabaseManager.addTagToImage(ImageName, pill.Text);
        }
        
        SetMessage("L'image a été ajoutée avec succès!", GREEN);
        ImageSource = null;
        ImageName = string.Empty;
        _imagePath = string.Empty;
    }
    
    private void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}