﻿using System;
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
        imageGrid.Children.Clear();
        imageGrid.RowDefinitions.Clear();
        imageGrid.ColumnDefinitions.Clear();
        
        imageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        for (int i = 0; i < NumColumns; i++)
        {
            imageGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        List<string> filters = new();
        foreach (Pill pill in pills)
        {
            filters.Add(pill.Text);
        }

        int counter = 0;
        int colCounter = 0;

        IEnumerable<ImagePhotocool> images;
        if (filters.Count == 0)
        {
            images = DatabaseManager.getAllImagesAsStream();
        }
        else
        {
            images = DatabaseManager.getImagesMustSatisfyAnyFilterAsStream(filters);
        }
        
        foreach (ImagePhotocool image in images)
        {
            if (colCounter >= NumColumns)
            {
                Console.WriteLine("Adding row!");
                imageGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                colCounter = 0;
            }
            
            int row = counter / NumColumns;
            int col = counter % NumColumns;
            Console.WriteLine(row + ", " + col);
            ImageCard imageCard = new(image.Name, image.Data);
            Grid.SetRow(imageCard, row);
            Grid.SetColumn(imageCard, col);
            imageGrid.Children.Add(imageCard);
            counter++;
            colCounter++;
        }
    }
}
