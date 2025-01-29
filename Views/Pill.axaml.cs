using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace photocool.Views;

public class Pill : Button 
{
    public Pill()
    {
        Random random = new();
        int r = random.Next(0, 200); // Values between 0 and 127
        int g = random.Next(0, 200);
        int b = random.Next(0, 200);
        Background = new SolidColorBrush(Color.FromRgb((byte)r,(byte)g,(byte)b));
        
        Console.WriteLine("PillTemplated instantiated !");
    }
}