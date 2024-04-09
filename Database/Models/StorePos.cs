namespace Project_CS412.Database.Models;

public partial class StorePos
{
    public int Cid { get; set; }

    public int StoreCid { get; set; }

    // public virtual ICollection<ItemInvLine> ItemInvLines { get; set; } = new List<ItemInvLine>();

    public virtual ICollection<ItemPickingLine> ItemPickingLines { get; set; } = new List<ItemPickingLine>();

    public virtual Store StoreC { get; set; } = null!;
}