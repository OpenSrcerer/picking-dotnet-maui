using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Project_CS412.Database;
using Project_CS412.Database.Models;
using Syncfusion.Maui.Data;

namespace Project_CS412.ViewModels;

public partial class DetailViewModel : ObservableObject
{
    public ObservableCollection<string> StorePosIdsComboBox { get; set; } = new();
    public ObservableCollection<string> ItemPackageIdsComboBox { get; set; } = new();
    public ObservableCollection<string> ItemOrderLineIdsComboBox { get; set; } = new();
    public ObservableCollection<ItemPickingLine> ItemPickingLines { get; } = new();

    [ObservableProperty] private ItemPicking _selectedItemPicking;

    private Dictionary<int, double> _availableQtyForEachIpl;
    private Dictionary<int, double> _consumedQtyForEachIol;

    private readonly DatabaseFacade _db;

    public DetailViewModel(DatabaseFacade databaseFacade)
    {
        _db = databaseFacade;
    }

    public void Create()
    {
        ItemPickingLines.Add(new ItemPickingLine
        {
            ItemPickingId = SelectedItemPicking.Id
        });
    }

    public void Delete(int index)
    {
        ItemPickingLines.RemoveAt(index);
    }

    public void Refresh()
    {
        Refresh(SelectedItemPicking);
    }

    public async void Refresh(ItemPicking itemPicking)
    {
        ItemOrderLineIdsComboBox.Clear();
        ItemPackageIdsComboBox.Clear();
        StorePosIdsComboBox.Clear();

        await Task.WhenAll(
            _db.ItemOrderLines.Where(iol => iol.ItemOrderId.Equals(itemPicking.ItemOrderId))
                .Select(iol => iol.Id.ToString())
                .ForEachAsync(ItemOrderLineIdsComboBox.Add),
            _db.ItemPkgs.Select(ipkg => ipkg.Id.ToString()).ForEachAsync(ItemPackageIdsComboBox.Add),
            _db.StorePoses.Select(sp => sp.Cid.ToString()).ForEachAsync(StorePosIdsComboBox.Add),
            ForEachItemComputeAvailableQuantity(itemPicking.Id)
        );

        SelectedItemPicking = itemPicking;
        var pickingLines =
            _db.ItemPickingLines.Where(ipl => ipl.ItemPickingId == itemPicking.Id);

        ItemPickingLines.Clear();
        pickingLines
            .ForEach(ipl =>
            {
                // Reloads values of entities (gets fresh copy from DB).
                // This is done to clear the quantity modifications
                _db.ItemPickingLines.Entry(ipl).Reload();

                ipl.QtyAvailable = _availableQtyForEachIpl[ipl.Id];
                ipl.QtyRemaining = ipl.QtyAvailable - _consumedQtyForEachIol[ipl.ItemOrderLineId];
                ItemPickingLines.Add(ipl);
            });
    }

    public async Task Save()
    {
        SelectedItemPicking.ItemPickingLines.Clear();
        ItemPickingLines.ForEach(ipl => SelectedItemPicking.ItemPickingLines.Add(ipl));
        _db.ItemPickings.Update(SelectedItemPicking);

        await _db.SaveChangesAsync();

        Refresh(SelectedItemPicking);
    }

    public void OnPickingChange(int iplIndex, double pickedQuantityDelta)
    {
        var selectedIpl = ItemPickingLines.ElementAt(iplIndex);
        if (selectedIpl.QtyRemaining + pickedQuantityDelta < 0)
        {
            throw new ArithmeticException("You cannot pick more than the available quantity!");
        }

        ItemPickingLines
            .Where(ipl => ipl.ItemOrderLineId.Equals(selectedIpl.ItemOrderLineId))
            .ForEach(ipl => { ipl.QtyRemaining += pickedQuantityDelta; });
    }

    private async Task ForEachItemComputeAvailableQuantity(int itemPickingId)
    {
        _availableQtyForEachIpl = await _db.ItemPickingLines
            .Where(ipl => ipl.ItemPickingId.Equals(itemPickingId))
            .Join(
                _db.ItemOrderLines,
                ipl => ipl.ItemOrderLineId,
                iol => iol.Id,
                (ipl, iol) => new
                {
                    ItemOrderLineId = iol.Id,
                    ItemPickingLineId = ipl.Id,
                    AvailableQty = iol.Qty
                }
            )
            .ToDictionaryAsync(
                rx => rx.ItemPickingLineId,
                rx => rx.AvailableQty
            );


        _consumedQtyForEachIol = await _db.ItemPickingLines
            .Where(ipl => ipl.ItemPickingId.Equals(itemPickingId))
            .Join(
                _db.ItemOrderLines,
                ipl => ipl.ItemOrderLineId,
                iol => iol.Id,
                (ipl, iol) => new
                {
                    ItemPickingLineId = ipl.Id,
                    ItemOrderLineId = iol.Id,
                    ConsumedQty = ipl.QtyInUnit
                }
            )
            .GroupBy(rx => rx.ItemOrderLineId)
            .Select(g => new
            {
                ItemOrderLineId = g.Key,
                ConsumedQty = g.Sum(rx => rx.ConsumedQty)
            })
            .ToDictionaryAsync(
                rx => rx.ItemOrderLineId,
                rx => rx.ConsumedQty
            );
    }
}