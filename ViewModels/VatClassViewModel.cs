using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using Project_CS412.Database;
using Project_CS412.Database.Models;

namespace Project_CS412.ViewModels;

public class VatClassViewModel
{
    public ObservableCollection<VatClass> VatClasses { get; } = new();

    public VatClassViewModel(DatabaseFacade db)
    {
        VatClasses = db.VatClasses.ToObservableCollection();
    }
}