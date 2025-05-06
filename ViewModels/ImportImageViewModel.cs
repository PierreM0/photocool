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
        
        long id = DatabaseManager.addImage(_imagePath);

        foreach (Pill pill in pills)
        {
            DatabaseManager.addTagToImage(id, pill.Text);
        }
        
        SetMessage("L'image a été ajoutée avec succès!", GREEN);
        ImageSource = null;
        _imagePath = string.Empty;
    }
    
    private void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}