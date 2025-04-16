using System;
using Avalonia.Media;

namespace photocool.ViewModels;

public partial class PillViewModel: ViewModel
{
    public IBrush Color { get; } = Brushes.Black; 
}
