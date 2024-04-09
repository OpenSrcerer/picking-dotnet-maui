namespace Project_CS412.Database.Models;

public partial class Store
{
    public int Cid { get; set; }

    public virtual ICollection<StorePos> StorePos { get; set; } = new List<StorePos>();
}