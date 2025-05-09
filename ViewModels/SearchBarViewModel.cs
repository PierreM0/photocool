using System.Collections.Generic;
using photocool.Views;

namespace photocool.ViewModels;

public class SearchBarViewModel : ViewModel
{
    public SearchBarViewModel()
    {
        TagRepository.Refresh();
    }
}