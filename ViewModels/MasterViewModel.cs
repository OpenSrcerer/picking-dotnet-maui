using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Project_CS412.Database;
using Project_CS412.Database.Models;

namespace Project_CS412.ViewModels;

public class MasterViewModel : ObservableObject
{
    public ObservableCollection<string> VatClassIdsComboBox { get; }
    public ObservableCollection<string> ItemOrderIdsComboBox { get; }
    public ObservableCollection<ItemPicking> ItemPickings { get; }
    public ItemPicking SelectedItemPicking { get; set; }
    private List<ItemPicking> _deletedPickings = new();

    private readonly DatabaseFacade _db;

    public MasterViewModel(DatabaseFacade databaseFacade)
    {
        _db = databaseFacade;

        VatClassIdsComboBox = new ObservableCollection<string>(
            _db.VatClasses.Select(vc => vc.Cid.ToString()));
        ItemOrderIdsComboBox = new ObservableCollection<string>(
            _db.ItemOrders.Select(io => io.Id.ToString()).ToList());
        ItemPickings = new ObservableCollection<ItemPicking>(_db.ItemPickings.ToList());
    }

    public void Create()
    {
        ItemPickings.Add(new ItemPicking
        {
            PickingStartDatetime = DateTime.Today,
            PickingEndDatetime = DateTime.Today
        });
    }

    public void Delete(int index)
    {
        _deletedPickings.Add(ItemPickings.ElementAt(index));
        ItemPickings.RemoveAt(index);
    }

    public async Task Refresh()
    {
        var vatClasses = await
            _db.VatClasses.Select(vc => vc.Cid.ToString()).ToListAsync();
        var itemOrders = await _db.ItemOrders.Select(io => io.Id.ToString()).ToListAsync();
        var pickings = await _db.ItemPickings.ToListAsync();

        VatClassIdsComboBox.Clear();
        ItemOrderIdsComboBox.Clear();
        itemOrders.ForEach(ItemOrderIdsComboBox.Add);
        vatClasses.ForEach(VatClassIdsComboBox.Add);

        ItemPickings.Clear();
        pickings.ForEach(ItemPickings.Add);
    }

    public async Task Save()
    {
        _deletedPickings.ForEach(p => _db.ItemPickings.Remove(p));
        Task.WaitAll(ItemPickings.Select(p => _db.ItemPickings.Upsert(p)).ToArray());
        await _db.SaveChangesAsync();
        _deletedPickings.Clear();
        await Refresh();
    }
}