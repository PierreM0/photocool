using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls;
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
    
    public int MaxRendered { get; set; } = 255;

    private static Dictionary<string, IBrush> pillColors = new();
    private static List<Pill> _list = new();
    public void Add(Pill pill)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            
            // toute les meme tags on une seule couleur
            if (pillColors.TryGetValue(pill.Text, out var color))
            {
                pill.PillColor = color;
            }
            else
            {
                pillColors.Add(pill.Text, pill.PillColor);
            }

            // pas deux fois le meme filtre dans la liste
            foreach (var p in _list)
            {
                if (p.Text == pill.Text) return;
            }

            _list.Add(pill);
            
            if (PillContainer.Children.Count < MaxRendered)
            {
                PillContainer.Children.Add(pill);
            } 
            else if (PillContainer.Children.Count == MaxRendered)
            {
                PillContainer.Children.Add(new Pill()
                {
                    Text = "...",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                });
            }
        });
    }
}