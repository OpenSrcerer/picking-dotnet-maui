using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Project_CS412.Database.Models;

[ObservableObject]
public partial class ItemPickingLine : AbstractItem
{
    public int ItemPickingId { get; set; }

    public int ItemOrderLineId { get; set; }

    public int ItemPkgId { get; set; }

    public int? StorePosCid { get; set; }

    public double QtyInUnit { get; set; }

    [NotMapped] public double QtyAvailable { get; set; }

    [NotMapped] private double _qtyRemaining;

    [NotMapped]
    public double QtyRemaining
    {
        get => _qtyRemaining;
        set => SetProperty(ref _qtyRemaining, value);
    }

    public virtual ItemOrderLine ItemOrderLine { get; set; } = null!;

    public virtual ItemPicking ItemPicking { get; set; } = null!;

    public virtual ItemPkg ItemPkg { get; set; } = null!;

    public virtual StorePos? StorePosC { get; set; }
}