using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace photocool.Views;

public partial class SearchBar: UserControl
{
    public SearchBar()
    {
        InitializeComponent();
    }

    private void AjoutFiltre_OnKeyUp(object? sender, KeyEventArgs e)
    {
    }

    private void NewFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        if (AjoutFiltre.Text == null)
            return;
        
        Pill pill = new();
        double h = Random.Shared.NextDouble() * 360;
        // (0..1) * 40 => (0..40) + 60 = (60..100) --> entre 0 et 1
        double s = (Random.Shared.NextDouble() * 0.40) + 0.60;
        // (0..1) * 20 => (0..20) + 20 = (20..40) --> entre 0 et 1
        double l = (Random.Shared.NextDouble() * 0.20) + 0.20;

        Color c = HslColor.FromHsl(h, s, l).ToRgb();
        pill.PillColor = new SolidColorBrush(c);

        
        pill.Text = AjoutFiltre.Text;
        
        pill.HorizontalAlignment = HorizontalAlignment.Center;
        pill.VerticalAlignment = VerticalAlignment.Center;
        
        PillsList.Add(pill);    
    }
}