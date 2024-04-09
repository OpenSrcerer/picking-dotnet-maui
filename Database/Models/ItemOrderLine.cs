namespace Project_CS412.Database.Models;

public partial class ItemOrderLine : AbstractItem
{
    public int ItemOrderId { get; set; }

    public int ItemId { get; set; }

    public double Qty { get; set; }

    // public virtual Item Item { get; set; } = null!;

    public virtual ItemOrder ItemOrder { get; set; } = null!;

    public virtual ICollection<ItemPickingLine> ItemPickingLines { get; set; } = new List<ItemPickingLine>();
}