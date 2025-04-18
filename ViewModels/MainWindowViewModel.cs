using System.Collections.Generic;
using Avalonia.Controls;
using photocool.DB;
using photocool.Models;
using photocool.Views;

namespace photocool.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private const int NumColumns = 4;
    
    public void HandleRefreshImageGrid(List<Pill> pills, Grid imageGrid)
    {
        List<ImagePhotocool> images = new();
        
        if (pills.Count == 0)
        {
            images = DatabaseManager.getImages();
        }
        
        int numImages = images.Count;
        int numRows = numImages / NumColumns + 1;
        
        for (int i = 0; i < numRows; i++)
        {
            imageGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }
        
        for (int i = 0; i < NumColumns; i++)
        {
            imageGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        int counter = 0;
        foreach (ImagePhotocool image in images)
        {
            int row = counter / NumColumns;
            int col = counter % NumColumns;
            ImageCard imageCard = new(image.Name, image.Data);
            Grid.SetRow(imageCard, row);
            Grid.SetColumn(imageCard, col);
            imageGrid.Children.Add(imageCard);
            counter++;
        }
    }
}
