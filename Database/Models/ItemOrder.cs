namespace Project_CS412.Database.Models;

public partial class ItemOrder : AbstractItem
{
    public bool IsCustomerOrder { get; set; }

    public int? CustomerId { get; set; }

    public int? SupplierId { get; set; }

    public DateTime OrderDatetime { get; set; }

    // public virtual Customer? Customer { get; set; }

    // public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<ItemOrderLine> ItemOrderLines { get; set; } = new List<ItemOrderLine>();

    public virtual ICollection<ItemPicking> ItemPickings { get; set; } = new List<ItemPicking>();

    // public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // public virtual Supplier? Supplier { get; set; }
}