using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.Swift;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Reactive;
using photocool.ViewModels;

namespace photocool.Views;

public partial class SearchBar: UserControl
{
    private SearchBarViewModel ViewModel = new();

    public Action OnPillClick { get; set; } = null;
    
    public SearchBar()
    {
        InitializeComponent();
        DataContext = ViewModel;
        AjoutFiltre.ItemsSource = TagRepository.Tags;
    }

    public void AddFilter(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return;

        if (!TagRepository.Contains(filter))
            return;
        
        Pill pill = new();
        double h = Random.Shared.NextDouble() * 360;
        // (0..1) * 40 => (0..40) + 60 = (60..100) --> entre 0 et 1
        double s = (Random.Shared.NextDouble() * 0.40) + 0.60;
        // (0..1) * 20 => (0..20) + 20 = (20..40) --> entre 0 et 1
        double l = (Random.Shared.NextDouble() * 0.20) + 0.20;

        Color c = HslColor.FromHsl(h, s, l).ToRgb();
        pill.PillColor = new SolidColorBrush(c);
        pill.Text = filter;
        
        pill.HorizontalAlignment = HorizontalAlignment.Center;
        pill.VerticalAlignment = VerticalAlignment.Center;
        
        PillsList.Add(pill, OnPillClick);

        AjoutFiltre.Text = "";
    }

    public void TryRemoveFilter(string filter)
    {
        Pill pillToRemove = null;
        foreach (Pill pill in PillsList.List)
        {
            if (pill.Text == filter)
            {
                pillToRemove = pill;
            }
        }

        if (pillToRemove == null)
        {
            return;
        }
        
        PillsList.List.Remove(pillToRemove);
        PillsList.PillContainer.Children.Remove(pillToRemove);
        OnPillClick();
    }

    private void NewFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        AddFilter(AjoutFiltre.Text);
    }
}