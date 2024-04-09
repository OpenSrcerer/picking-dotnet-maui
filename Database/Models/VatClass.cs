namespace Project_CS412.Database.Models;

public partial class VatClass
{
    public int Cid { get; set; }
    
    public string ShortCode { get; set; } = null!;
    
    public string ClassDescription { get; set; } = null!;
    
    public int IsActive { get; set; }
    
    public bool IsActiveBool => Convert.ToBoolean(IsActive);

    // References
    
    public virtual ICollection<ItemPicking> ItemPickings { get; set; } = new List<ItemPicking>();
}