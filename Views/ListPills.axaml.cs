using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace photocool.Views;

public partial class ListPills : UserControl
{
    
    
    public ListPills()
    {
        InitializeComponent();
    }

    private static Dictionary<string, IBrush> pillColors = new();
    public List<Pill> List { get; }= new();
    public void Add(Pill pill)
    {
        
        pill.AddHandler(PointerPressedEvent, ((object sender, PointerPressedEventArgs e) =>
        {
            List.Remove(pill);
            PillContainer.Children.Remove(pill);
        }), RoutingStrategies.Tunnel);
        
        Dispatcher.UIThread.Invoke(() =>
        {
            if (pillColors.TryGetValue(pill.Text, out var color))
            {
                pill.PillColor = color;
            }
            else
            {
                pillColors.Add(pill.Text, pill.PillColor);
            }

            // pas deux fois le meme filtre dans la liste
            foreach (var p in List)
            {
                if (p.Text == pill.Text) return;
            }

            List.Add(pill);
            PillContainer.Children.Add(pill);
        });
    }
}
