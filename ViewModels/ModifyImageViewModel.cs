using Avalonia.Media.Imaging;

namespace photocool.ViewModels;

public class ModifyImageViewModel : ViewModel
{
    private Bitmap _imageSource;
    public Bitmap ImageSource
    {
        get => _imageSource;
        set { _imageSource = value; OnPropertyChanged(nameof(ImageSource)); }
    }
    
    public ModifyImageViewModel()
    {
        
    }
}