using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Project_CS412.Database;
using Project_CS412.Database.Models;
using Syncfusion.Maui.DataSource.Extensions;

namespace Project_CS412.ViewModels;

public partial class ItemPkgViewModel : ObservableObject
{
    public ObservableCollection<ItemPkg> ItemPkgs { get; } = new();

    public ItemPkgViewModel(DatabaseFacade db)
    {
        ItemPkgs = db.ItemPkgs.ToObservableCollection();
    }
}