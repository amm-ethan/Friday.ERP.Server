using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.Data.Entities.SystemManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure;

public class RepositoryContext(DbContextOptions<RepositoryContext> options) : DbContext(options)
{
    public DbSet<User>? Users { get; init; }
    public DbSet<UserLogin>? UserLogins { get; init; }
    public DbSet<UserRole>? UserRoles { get; init; }

    public DbSet<CustomerVendor>? CustomersVendors { get; init; }

    public DbSet<Category>? Categories { get; init; }
    public DbSet<Product>? Products { get; init; }
    public DbSet<ProductPrice>? ProductPrices { get; init; }

    public DbSet<InvoicePurchase>? InvoicePurchases { get; init; }
    public DbSet<InvoicePurchaseProduct>? InvoicePurchaseProducts { get; init; }

    public DbSet<InvoiceSale>? InvoiceSales { get; init; }
    public DbSet<InvoiceSaleProduct>? InvoiceSaleProducts { get; init; }
    public DbSet<InvoiceSaleDelivery>? InvoiceSaleDeliveries { get; init; }

    public DbSet<Notification>? Notifications { get; init; }
    public DbSet<NotificationUser>? NotificationUsers { get; init; }
    public DbSet<Setting>? Settings { get; init; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region User Management

        builder.Entity<User>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<User>().HasIndex(x => x.Phone).IsUnique();
        builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<User>().HasIndex(x => x.Username).IsUnique();

        builder.Entity<UserLogin>().HasIndex(x => x.AccessToken).IsUnique();
        builder.Entity<UserLogin>().HasIndex(x => x.RefreshToken).IsUnique();

        builder.Entity<User>()
            .HasOne<UserLogin>(c => c.UserLogin)
            .WithOne(e => e.User)
            .HasForeignKey<UserLogin>(e => e.UserGuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserRole>().HasIndex(x => x.Name).IsUnique();

        builder.Entity<User>()
            .HasOne<UserRole>(c => c.UserRole)
            .WithMany(e => e.Users)
            .HasForeignKey(x => x.UserRoleGuid)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Customer Vendor Management

        builder.Entity<CustomerVendor>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<CustomerVendor>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<CustomerVendor>().HasIndex(x => x.Phone).IsUnique();

        builder.Entity<CustomerVendor>()
            .HasMany(c => c.InvoicePurchases)
            .WithOne(e => e.Vendor)
            .HasForeignKey(x => x.VendorGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CustomerVendor>()
            .HasMany(c => c.InvoiceSales)
            .WithOne(e => e.Customer)
            .HasForeignKey(x => x.CustomerGuid)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Inventory Management

        builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Product>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<Product>().HasIndex(x => x.Name).IsUnique();

        builder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(e => e.Category)
            .HasForeignKey(x => x.CategoryGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(c => c.ProductPrices)
            .WithOne(e => e.Product)
            .HasForeignKey(x => x.ProductGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(c => c.InvoicePurchaseProducts)
            .WithOne(e => e.Product)
            .HasForeignKey(x => x.ProductGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(c => c.InvoiceSaleProducts)
            .WithOne(e => e.Product)
            .HasForeignKey(x => x.ProductGuid)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Leger Management

        builder.Entity<InvoicePurchase>()
            .HasMany(c => c.PurchasedProducts)
            .WithOne(e => e.InvoicePurchase)
            .HasForeignKey(x => x.InvoicePurchaseGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<InvoiceSale>()
            .HasMany(c => c.SoldProducts)
            .WithOne(e => e.InvoiceSale)
            .HasForeignKey(x => x.InvoiceSaleGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<InvoiceSale>()
            .HasOne<InvoiceSaleDelivery>(c => c.InvoiceSaleDelivery)
            .WithOne(e => e.InvoiceSale)
            .HasForeignKey<InvoiceSaleDelivery>(x => x.InvoiceSaleGuid)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Notification Management

        builder.Entity<Notification>()
            .HasMany(c => c.NotificationUsers)
            .WithOne(e => e.Notification)
            .HasForeignKey(x => x.NotificationGuid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<NotificationUser>()
            .HasOne<User>(c => c.User)
            .WithMany(e => e.NotificationUsers)
            .HasForeignKey(x => x.UserGuid)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Data Seeds

        builder.Entity<UserRole>().HasData(new UserRole
        {
            Guid = Guid.Parse("7d5ac91f-48ab-4ec7-883e-ba83a13e26bb"),
            Name = "Admin"
        });

        builder.Entity<User>().HasData(new User
        {
            Guid = Guid.Parse("55b36e77-3ac8-46d8-9611-d1987d8041be"),
            Name = "Admin",
            Email = "Admin@admin.com",
            Username = "admin",
            Password = "$2a$11$BDyDgdnLAS9iFc2K/kR.R.RvOoKWCVOExGk708Qa5FlsO6Sn9x7ze",
            UserRoleGuid = Guid.Parse("7d5ac91f-48ab-4ec7-883e-ba83a13e26bb")
        });

        builder.Entity<Setting>().HasData(new Setting
        {
            Guid = Guid.Parse("b1819545-6b5b-4114-8b66-76065037b1de"),
            Image = "logo.png",
            Name = "MK Trading",
            AddressOne = "အမှတ်(၃၁), တိုးချဲ့ကမ်းနားလမ်း, မြိတ်တံတားထိပ်, ဖက်တန်းရပ်, မော်လမြိုင်မြို့",
            AddressTwo = "အမှတ်(၁၈), ကျိုက်သန်လန် ဘုရားလမ်း, ရွှေတောင်ရပ်, မော်လမြိုင်မြို့",
            PhoneOne = "09-446639594",
            PhoneTwo = "09-777308660",
            PhoneThree = "09-760142884",
            PhoneFour = "057-24258",
            DefaultProfitPercent = 15,
            DefaultProfitPercentForWholeSale = 10,
            SuggestSalePrice = true,
            MinimumStockMargin = 10
        });

        #endregion
    }
}