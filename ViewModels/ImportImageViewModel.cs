using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using photocool.DB;
using photocool.Models;
using photocool.Views;

namespace photocool.ViewModels;

public class ImportImageViewModel : ViewModel
{
    private List<string> _imagePaths = new();
    
    public ObservableCollection<Bitmap> ImagePreviews { get; set; } = new();
    
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
        _message = string.Empty;
    }

    public async void HandleSelectImage(Window parentWindow)
    {
        OpenFileDialog dialog = new();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpeg", "jpg" } });
        dialog.AllowMultiple = true;
        
        string[]? result = await dialog.ShowAsync(parentWindow);
        if (result != null && result.Length > 0)
        {
            foreach (string path in result)
            {
                Bitmap bitmap = new Bitmap(new MemoryStream(ThumbnailPhotocool.CreateThumbnailFromData(File.ReadAllBytes(path))));
                ImagePreviews.Add(bitmap);
                _imagePaths.Add(path);
            }
        }
    }

    public void HandleImport(List<Pill> pills)
    {
        if (_imagePaths.Count == 0)
        {
            SetMessage("Veuillez sélectionner une ou plusieurs images!", RED);
            return;
        }

        foreach (string path in _imagePaths)
        {
            long id = DatabaseManager.addImage(path);

            foreach (Pill pill in pills)
            {
                DatabaseManager.addTagToImage(id, pill.Text);
            }
        }
        
        SetMessage("Succès de l'importation!", GREEN);
        ImagePreviews.Clear();
        _imagePaths.Clear();
    }
    
    private void SetMessage(string message, Brush color)
    {
        Message = message;
        MessageColor = color;
    }
}