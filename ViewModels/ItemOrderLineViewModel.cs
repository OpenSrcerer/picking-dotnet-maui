using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Project_CS412.Database;
using Project_CS412.Database.Models;

namespace Project_CS412.ViewModels;

public partial class ItemOrderLineViewModel : ObservableObject
{
    public ObservableCollection<ItemOrderLine> ItemOrderLines { get; } = new();

    public ItemOrderLineViewModel(DatabaseFacade db)
    {
        ItemOrderLines = db.ItemOrderLines.ToObservableCollection();
    }
}