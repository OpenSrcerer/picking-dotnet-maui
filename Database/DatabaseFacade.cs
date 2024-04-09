using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project_CS412.Database.Models;

namespace Project_CS412.Database;

public partial class DatabaseFacade : DbContext
{
    public string DbPath { get; }

    public DatabaseFacade(IConfiguration configuration)
    {
        var dbConfig = configuration.GetRequiredSection("Database").Get<DatabaseConfig>();
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        DbPath = Path.Join(path, dbConfig.Name);

        Database.EnsureCreated();
        try
        {
            if (ItemOrders.ToList().Count() == 0)
            {
                OnStartupMockData();
            }
        }
        catch (Exception ex)
        {
            OnStartupMockData();
        }
    }

    public virtual DbSet<ItemOrder> ItemOrders { get; set; }

    public virtual DbSet<ItemOrderLine> ItemOrderLines { get; set; }
    public virtual DbSet<ItemPicking> ItemPickings { get; set; }

    public virtual DbSet<ItemPickingLine> ItemPickingLines { get; set; }

    public virtual DbSet<ItemPkg> ItemPkgs { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StorePos> StorePoses { get; set; }
    
    public virtual DbSet<VatClass> VatClasses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemOrder>(entity =>
        {
            entity.ToTable("ITEM_ORDER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.IsCustomerOrder).HasColumnName("IS_CUSTOMER_ORDER");
            entity.Property(e => e.OrderDatetime)
                .HasColumnType("datetime")
                .HasColumnName("ORDER_DATETIME");
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");

            // entity.HasOne(d => d.Customer).WithMany(p => p.ItemOrders)
            //     .HasForeignKey(d => d.CustomerId)
            //     .HasConstraintName("FK_ITEM_ORDER_REF_CUSTOMER");
            //
            // entity.HasOne(d => d.Supplier).WithMany(p => p.ItemOrders)
            //     .HasForeignKey(d => d.SupplierId)
            //     .HasConstraintName("FK_ITEM_ORDER_REF_SUPPLIER");
        });

        modelBuilder.Entity<ItemOrderLine>(entity =>
        {
            entity.ToTable("ITEM_ORDER_LINE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.ItemOrderId).HasColumnName("ITEM_ORDER_ID");
            entity.Property(e => e.Qty).HasColumnName("QTY");

            // entity.HasOne(d => d.Item).WithMany(p => p.ItemOrderLines)
            //     .HasForeignKey(d => d.ItemId)
            //     .OnDelete(DeleteBehavior.Cascade)
            //     .HasConstraintName("FK_ITEM_ORDER_LINE_REF_ITEM");

            entity.HasOne(d => d.ItemOrder).WithMany(p => p.ItemOrderLines)
                .HasForeignKey(d => d.ItemOrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ITEM_ORDER_LINE_REF_ITEM_ORDER");
        });

        modelBuilder.Entity<ItemPicking>(entity =>
        {
            entity.ToTable("ITEM_PICKING");

            entity.Property(e => e.Id).HasColumnName("ID");
            // entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.VatClassId).HasColumnName("VAT_CLASS_ID");
;            entity.Property(e => e.ItemOrderId).HasColumnName("ITEM_ORDER_ID");
            entity.Property(e => e.PickingEndDatetime)
                .HasColumnType("datetime")
                .HasColumnName("PICKING_END_DATETIME");
            entity.Property(e => e.PickingStartDatetime)
                .HasColumnType("datetime")
                .HasColumnName("PICKING_START_DATETIME");

            // entity.HasOne(d => d.Item).WithMany(p => p.ItemPickings)
            //     .HasForeignKey(d => d.ItemId)
            //     .OnDelete(DeleteBehavior.Cascade)
            //     .HasConstraintName("FK_ITEM_PICKING_REF_ITEM");

            entity.HasOne(d => d.ItemOrder)
                .WithMany(p => p.ItemPickings)
                .HasForeignKey(d => d.ItemOrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ITEM_PICKING_REF_ITEM_ORDER");
            
            entity.HasOne(d => d.VatClass)
                .WithMany(p => p.ItemPickings)
                .HasForeignKey(d => d.VatClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ITEM_PICKING_REF_VAT_CLASS");
        });

        modelBuilder.Entity<ItemPickingLine>(entity =>
        {
            entity.ToTable("ITEM_PICKING_LINE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemOrderLineId).HasColumnName("ITEM_ORDER_LINE_ID");
            entity.Property(e => e.ItemPickingId).HasColumnName("ITEM_PICKING_ID");
            entity.Property(e => e.ItemPkgId).HasColumnName("ITEM_PKG_ID");
            entity.Property(e => e.QtyInUnit).HasColumnName("QTY_IN_UNIT");
            entity.Property(e => e.StorePosCid).HasColumnName("STORE_POS_CID");

            entity.HasOne(d => d.ItemOrderLine)
                .WithMany(p => p.ItemPickingLines)
                .HasForeignKey(d => d.ItemOrderLineId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ITEM_PICKING_LINE_REF_ITEM_ORDER_LINE");

            entity.HasOne(d => d.ItemPicking)
                .WithMany(p => p.ItemPickingLines)
                .HasForeignKey(d => d.ItemPickingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ITEM_PICKING_LINE_REF_ITEM_PICKING");

            entity.HasOne(d => d.ItemPkg)
                .WithMany(p => p.ItemPickingLines)
                .HasForeignKey(d => d.ItemPkgId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ITEM_PICKING_LINE_REF_ITEM_PKG");

            entity.HasOne(d => d.StorePosC)
                .WithMany(p => p.ItemPickingLines)
                .HasForeignKey(d => d.StorePosCid)
                .HasConstraintName("FK_ITEM_PICKING_LINE_REF_STORE_POS");
        });

        modelBuilder.Entity<ItemPkg>(entity =>
        {
            entity.ToTable("ITEM_PKG");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Barcode)
                .HasMaxLength(32)
                .HasColumnName("BARCODE");
            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.SerialNum).HasColumnName("SERIAL_NUM");

            // entity.HasOne(d => d.Item).WithMany(p => p.ItemPkgs)
            //     .HasForeignKey(d => d.ItemId)
            //     .OnDelete(DeleteBehavior.Cascade)
            //     .HasConstraintName("FK_ITEM_PKG_REF_ITEM");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Cid);

            entity.ToTable("STORE");

            entity.Property(e => e.Cid)
                .ValueGeneratedNever()
                .HasColumnName("CID");
        });

        modelBuilder.Entity<StorePos>(entity =>
        {
            entity.HasKey(e => e.Cid);

            entity.ToTable("STORE_POS");

            entity.Property(e => e.Cid)
                .ValueGeneratedNever()
                .HasColumnName("CID");
            entity.Property(e => e.StoreCid).HasColumnName("STORE_CID");

            entity.HasOne(d => d.StoreC).WithMany(p => p.StorePos)
                .HasForeignKey(d => d.StoreCid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_STORE_POS_STORE");
        });
        
        modelBuilder.Entity<VatClass>(entity =>
        {
            entity.ToTable("VAT_CLASS");

            entity.HasKey(e => e.Cid);

            entity.Property(e => e.Cid).HasColumnName("CID");
            entity.Property(e => e.ShortCode).HasColumnName("SHORT_CODE");
            entity.Property(e => e.ClassDescription).HasColumnName("CLASS_DESCRIPTION");
            entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private void OnStartupMockData()
    {
        ItemOrders.AddRange(
            new ItemOrder
            {
                Id = 365,
                IsCustomerOrder = false,
                CustomerId = 1234,
                SupplierId = 6789,
                OrderDatetime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(180))
            },
            new ItemOrder
            {
                Id = 3328,
                IsCustomerOrder = true,
                CustomerId = 666,
                SupplierId = 777,
                OrderDatetime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(360))
            }
        );

        ItemOrderLines.AddRange(
            new ItemOrderLine
            {
                ItemOrderId = 365,
                ItemId = 318,
                Qty = 22270.33
            },
            new ItemOrderLine
            {
                ItemOrderId = 365,
                ItemId = 54323,
                Qty = 10
            },
            new ItemOrderLine
            {
                ItemOrderId = 3328,
                ItemId = 100,
                Qty = 7
            },
            new ItemOrderLine
            {
                ItemOrderId = 3328,
                ItemId = 101,
                Qty = 3
            }
        );

        ItemPkgs.AddRange(
            new ItemPkg
            {
                ItemId = 318,
                Barcode = "333030033303030",
                SerialNum = 10000000000392
            },
            new ItemPkg
            {
                ItemId = 54323,
                Barcode = "403083944003923",
                SerialNum = 10000000000333
            },
            new ItemPkg
            {
                ItemId = 100,
                Barcode = "318391319001900",
                SerialNum = 10000000000343
            },
            new ItemPkg
            {
                ItemId = 101,
                Barcode = "131183913010010",
                SerialNum = 10000000000344
            }
        );
        Stores.AddRange(
            new Store { Cid = 1 },
            new Store { Cid = 2 },
            new Store { Cid = 3 },
            new Store { Cid = 4 },
            new Store { Cid = 666 }
        );

        StorePoses.AddRange(
            new StorePos { Cid = 1, StoreCid = 1 },
            new StorePos { Cid = 2, StoreCid = 1 },
            new StorePos { Cid = 3, StoreCid = 1 },
            new StorePos { Cid = 4, StoreCid = 1 },
            new StorePos { Cid = 5, StoreCid = 1 },
            new StorePos { Cid = 6, StoreCid = 2 },
            new StorePos { Cid = 7, StoreCid = 2 },
            new StorePos { Cid = 8, StoreCid = 3 },
            new StorePos { Cid = 9, StoreCid = 3 },
            new StorePos { Cid = 10, StoreCid = 3 },
            new StorePos { Cid = 11, StoreCid = 3 },
            new StorePos { Cid = 12, StoreCid = 4 },
            new StorePos { Cid = 13, StoreCid = 4 },
            new StorePos { Cid = 14, StoreCid = 4 },
            new StorePos { Cid = 15, StoreCid = 4 },
            new StorePos { Cid = 16, StoreCid = 4 },
            new StorePos { Cid = 17, StoreCid = 666 },
            new StorePos { Cid = 18, StoreCid = 666 },
            new StorePos { Cid = 19, StoreCid = 666 },
            new StorePos { Cid = 20, StoreCid = 666 }
        );
        
        VatClasses.AddRange(
            new VatClass { Cid = 1, ShortCode = "KF24", ClassDescription = "Normal Tax Rate 24%", IsActive = 1 },
            new VatClass { Cid = 2, ShortCode = "KF13", ClassDescription = "Normal Tax Rate 13%", IsActive = 0 },
            new VatClass { Cid = 3, ShortCode = "AF6", ClassDescription = "Recognized Tax Rate 6%", IsActive = 1 },
            new VatClass { Cid = 4, ShortCode = "AF0", ClassDescription = "Tax Exemption", IsActive = 0 },
            new VatClass { Cid = 5, ShortCode = "VER", ClassDescription = "Verified Tax Rate", IsActive = 1 }
        );

        SaveChanges();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}