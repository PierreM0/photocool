using System;
using Avalonia.Controls;

namespace photocool.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        const int NB_IMAGES = 25;
        const int NB_COLUMNS = 4;
        const int NB_ROWS = 7;

        for (int i = 0; i < NB_ROWS; i++)
        {
            Grille.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
        }
        
        for (int i = 0; i < NB_COLUMNS; i++)
        {
            Grille.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
        }

        for (int i = 0; i < NB_IMAGES; i++)
        {
            int row = i / NB_COLUMNS;
            int col = i % NB_COLUMNS;
            Console.WriteLine(row + ", " + col);
            CaseGrille caseGrille = new CaseGrille();
            Grid.SetRow(caseGrille, row);
            Grid.SetColumn(caseGrille, col);
            Grille.Children.Add(caseGrille);
        }
    }
}