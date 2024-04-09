namespace Project_CS412.Database.Models;

public partial class ItemPicking : AbstractItem
{
    // public int ItemId { get; set; }

    public int ItemOrderId { get; set; }
    
    public int VatClassId { get; set; }

    public DateTime PickingStartDatetime { get; set; }

    public DateTime PickingEndDatetime { get; set; }

    // public virtual Item Item { get; set; } = null!;

    public virtual ItemOrder ItemOrder { get; set; } = null!;
    
    public virtual VatClass VatClass { get; set; } = null!;

    public virtual ICollection<ItemPickingLine> ItemPickingLines { get; set; } = new List<ItemPickingLine>();
}