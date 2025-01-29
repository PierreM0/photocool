using System;
using Avalonia.Media;

namespace photocool.ViewModels;

public partial class PillViewModel: ViewModelBase
{
    public IBrush Color { get; } = Brushes.Black; 
}
