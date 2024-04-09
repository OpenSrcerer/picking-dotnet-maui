using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Project_CS412.Database;
using Project_CS412.Database.Models;
using Syncfusion.Maui.DataSource.Extensions;

namespace Project_CS412.ViewModels;

public partial class StoreViewModel : ObservableObject
{
    public ObservableCollection<Store> Stores { get; } = new();

    public StoreViewModel(DatabaseFacade db)
    {
        Stores = db.Stores.ToObservableCollection();
    }
}