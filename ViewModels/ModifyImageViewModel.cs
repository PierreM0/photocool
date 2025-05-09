using System.Collections.Generic;
using System.IO;
using Avalonia.Media.Imaging;
using photocool.DB;
using photocool.Views;

namespace photocool.ViewModels;

public class ModifyImageViewModel : ViewModel
{
    private Bitmap _imageSource;
    public Bitmap ImageSource
    {
        get => _imageSource;
        set { _imageSource = value; OnPropertyChanged(nameof(ImageSource)); }
    }
    
    private long Id { get; set; }
    
    public ModifyImageViewModel(long id)
    {
        ImageSource = new Bitmap(new MemoryStream(DatabaseManager.getImage(id)));
        Id = id;
    }

    public void ExecuteModify(List<Pill> pills)
    {
        DatabaseManager.RemoveImageTags(Id);

        foreach (Pill pill in pills)
        {
            string tag = pill.Text;
            DatabaseManager.addTagToImage(Id, tag);
        }
    }
}