using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using photocool.ViewModels;

namespace photocool.Views;

public partial class Pill : UserControl 
{
    public Pill()
    {
        Margin = new Thickness(5);
        InitializeComponent();
    }

    private static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Pill, string>(nameof(Text), string.Empty);
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static readonly StyledProperty<IBrush> PillColorProperty =
        AvaloniaProperty.Register<Pill, IBrush>(nameof(PillColor), Brushes.Gray); 
    
    public IBrush PillColor
    {
        get => GetValue(PillColorProperty);
        set => SetValue(PillColorProperty, value);
    }
}