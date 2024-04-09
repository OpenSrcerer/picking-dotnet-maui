namespace Project_CS412.Database.Models;

public partial class ItemPkg : AbstractItem
{
    public int ItemId { get; set; }

    public string Barcode { get; set; } = null!;

    public long SerialNum { get; set; }

    // public virtual Item Item { get; set; } = null!;

    // public virtual ICollection<ItemInvLine> ItemInvLines { get; set; } = new List<ItemInvLine>();

    public virtual ICollection<ItemPickingLine> ItemPickingLines { get; set; } = new List<ItemPickingLine>();

    // public virtual ICollection<Production> Productions { get; set; } = new List<Production>();
}