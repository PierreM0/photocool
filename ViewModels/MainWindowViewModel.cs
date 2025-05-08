using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using photocool.DB;
using photocool.Models;
using photocool.Views;

namespace photocool.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private const int NumColumns = 8;

    public ObservableCollection<TagNode> TagNodes => TagRepository.TagNodes;
    
    public void HandleRefreshImageGrid(List<Pill> pills, WrapPanel imagePanel, bool allFilters)
    {
        imagePanel.Children.Clear();

        List<string> filters = new();
        foreach (Pill pill in pills)
        {
            filters.Add(pill.Text);
        }
        
        IEnumerable<ThumbnailPhotocool> images;
        if (filters.Count == 0)
        {
            images = DatabaseManager.getAllImagesAsStream();
        }
        else if (allFilters)
        {
            images = DatabaseManager.getImagesMustSatisfyAllFiltersAsStream(filters);
        }
        else
        {
            images = DatabaseManager.getImagesMustSatisfyAnyFilterAsStream(filters);
        }
        
        foreach (ThumbnailPhotocool image in images)
        {
            ImageCard imageCard = new(image.Id, image.Data, () => HandleRefreshImageGrid(pills, imagePanel, allFilters))
            {
                Width = 140,
                Height = 140,
                Margin = new Thickness(5)
            };
            imagePanel.Children.Add(imageCard);
        }
    }

    public void ExecuteDeparentTag(object? tag)
    {
        DatabaseManager.removeParentFromTag((tag as TagNode).Tag);
        Console.WriteLine((tag as TagNode).Tag);
        TagRepository.Refresh();
    }

    public void ExecuteDeleteTag(object? tag)
    {
        DatabaseManager.removeTag((tag as TagNode).Tag);
        TagRepository.Refresh();
    }
}
