using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Project_CS412.Database;
using Project_CS412.Database.Models;

namespace Project_CS412.ViewModels;

public partial class StorePosViewModel : ObservableObject
{
    public ObservableCollection<StorePos> StorePoses { get; } = new();

    public StorePosViewModel(DatabaseFacade db)
    {
        StorePoses = db.StorePoses.ToObservableCollection();
    }
}