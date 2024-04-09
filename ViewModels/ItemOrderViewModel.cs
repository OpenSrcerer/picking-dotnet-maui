using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Project_CS412.Database;
using Project_CS412.Database.Models;
using Syncfusion.Maui.DataSource.Extensions;

namespace Project_CS412.ViewModels;

public partial class ItemOrderViewModel : ObservableObject
{
    public ObservableCollection<ItemOrder> ItemOrders { get; } = new();

    public ItemOrderViewModel(DatabaseFacade db)
    {
        ItemOrders = db.ItemOrders.ToObservableCollection();
    }
}