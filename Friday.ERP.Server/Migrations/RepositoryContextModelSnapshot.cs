﻿// <auto-generated />
using System;
using Friday.ERP.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Friday.ERP.Server.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("phone");

                    b.Property<Guid?>("UserRoleGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("user_role_guid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("username");

                    b.HasKey("Guid");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("UserRoleGuid");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("am_user");

                    b.HasData(
                        new
                        {
                            Guid = new Guid("55b36e77-3ac8-46d8-9611-d1987d8041be"),
                            Email = "Admin@admin.com",
                            Name = "Admin",
                            Password = "$2a$11$BDyDgdnLAS9iFc2K/kR.R.RvOoKWCVOExGk708Qa5FlsO6Sn9x7ze",
                            UserRoleGuid = new Guid("7d5ac91f-48ab-4ec7-883e-ba83a13e26bb"),
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.UserLogin", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("varchar(512)")
                        .HasColumnName("access_token");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("varchar(512)")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime>("RefreshTokenExpiryAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("refresh_token_expiry_at");

                    b.Property<Guid?>("UserGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("user_guid");

                    b.HasKey("Guid");

                    b.HasIndex("AccessToken")
                        .IsUnique();

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.HasIndex("UserGuid")
                        .IsUnique();

                    b.ToTable("am_user_login");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.UserRole", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Guid");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("am_user_role");

                    b.HasData(
                        new
                        {
                            Guid = new Guid("7d5ac91f-48ab-4ec7-883e-ba83a13e26bb"),
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.CustomerVendorManagement.CustomerVendor", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("address");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("code");

                    b.Property<int>("CustomerVendorType")
                        .HasColumnType("int")
                        .HasColumnName("customer_vendor_type");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasColumnName("phone");

                    b.Property<long>("TotalCreditDebitLeft")
                        .HasColumnType("bigint")
                        .HasColumnName("total_credit_debit_left");

                    b.HasKey("Guid");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("cm_customer_vendor");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.Category", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("color");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("name");

                    b.HasKey("Guid");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("im_category");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.Product", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CategoryGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("category_guid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("code");

                    b.Property<string>("Image")
                        .HasColumnType("longtext")
                        .HasColumnName("image");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("name");

                    b.Property<int>("Stock")
                        .HasColumnType("int")
                        .HasColumnName("stock");

                    b.HasKey("Guid");

                    b.HasIndex("CategoryGuid");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("im_product");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.ProductPrice", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ActionAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("action_at");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsWholeSale")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_whole_sale");

                    b.Property<Guid?>("ProductGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("product_guid");

                    b.Property<long>("SalePrice")
                        .HasColumnType("bigint")
                        .HasColumnName("sale_price");

                    b.HasKey("Guid");

                    b.HasIndex("ProductGuid");

                    b.ToTable("im_product_price");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoicePurchase", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<long>("CreditDebitLeft")
                        .HasColumnType("bigint")
                        .HasColumnName("credit_debit_left");

                    b.Property<long>("Discount")
                        .HasColumnType("bigint")
                        .HasColumnName("discount");

                    b.Property<int?>("DiscountType")
                        .HasColumnType("int")
                        .HasColumnName("discount_type");

                    b.Property<string>("InvoiceNo")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("invoice_no");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_paid");

                    b.Property<long>("PaidTotal")
                        .HasColumnType("bigint")
                        .HasColumnName("paid_total");

                    b.Property<DateTime>("PurchasedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("purchased_at");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext")
                        .HasColumnName("remark");

                    b.Property<long>("SubTotal")
                        .HasColumnType("bigint")
                        .HasColumnName("sub_total");

                    b.Property<long>("Total")
                        .HasColumnType("bigint")
                        .HasColumnName("total");

                    b.Property<Guid>("VendorGuid")
                        .HasColumnType("char(36)")
                        .HasColumnName("vendor_guid");

                    b.HasKey("Guid");

                    b.HasIndex("VendorGuid");

                    b.ToTable("lm_invoice_purchase");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoicePurchaseProduct", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("InvoicePurchaseGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("invoice_purchase_guid");

                    b.Property<Guid?>("ProductGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("product_guid");

                    b.Property<DateTime>("PurchasedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("purchased_at");

                    b.Property<long>("PurchasedPrice")
                        .HasColumnType("bigint")
                        .HasColumnName("purchased_price");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<long>("TotalPrice")
                        .HasColumnType("bigint")
                        .HasColumnName("total_price");

                    b.HasKey("Guid");

                    b.HasIndex("InvoicePurchaseGuid");

                    b.HasIndex("ProductGuid");

                    b.ToTable("lm_invoice_purchase_product");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSale", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<long>("CreditDebitLeft")
                        .HasColumnType("bigint")
                        .HasColumnName("credit_debit_left");

                    b.Property<Guid?>("CustomerGuid")
                        .HasColumnType("char(36)")
                        .HasColumnName("customer_guid");

                    b.Property<long>("Discount")
                        .HasColumnType("bigint")
                        .HasColumnName("discount");

                    b.Property<int?>("DiscountType")
                        .HasColumnType("int")
                        .HasColumnName("discount_type");

                    b.Property<string>("InvoiceNo")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("invoice_no");

                    b.Property<long>("PaidTotal")
                        .HasColumnType("bigint")
                        .HasColumnName("paid_total");

                    b.Property<DateTime>("PurchasedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("purchased_at");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext")
                        .HasColumnName("remark");

                    b.Property<long>("SubTotal")
                        .HasColumnType("bigint")
                        .HasColumnName("sub_total");

                    b.Property<long>("Total")
                        .HasColumnType("bigint")
                        .HasColumnName("total");

                    b.HasKey("Guid");

                    b.HasIndex("CustomerGuid");

                    b.ToTable("lm_invoice_sale");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSaleDelivery", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("address");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("contact_person");

                    b.Property<string>("ContactPhone")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("contact_phone");

                    b.Property<string>("DeliveryContactPerson")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("delivery_contact_person");

                    b.Property<string>("DeliveryContactPhone")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("delivery_contact_phone");

                    b.Property<string>("DeliveryFees")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("delivery_fees");

                    b.Property<string>("DeliveryServiceName")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("delivery_service_name");

                    b.Property<Guid?>("InvoiceSaleGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("invoice_sale_guid");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("remark");

                    b.HasKey("Guid");

                    b.HasIndex("InvoiceSaleGuid")
                        .IsUnique();

                    b.ToTable("lm_invoice_sale_delivery");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSaleProduct", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("InvoiceSaleGuid")
                        .HasColumnType("char(36)")
                        .HasColumnName("invoice_sale_guid");

                    b.Property<Guid>("ProductGuid")
                        .HasColumnType("char(36)")
                        .HasColumnName("product_guid");

                    b.Property<Guid>("ProductPriceGuid")
                        .HasColumnType("char(36)")
                        .HasColumnName("product_price_guid");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<long>("TotalPrice")
                        .HasColumnType("bigint")
                        .HasColumnName("total_price");

                    b.HasKey("Guid");

                    b.HasIndex("InvoiceSaleGuid");

                    b.HasIndex("ProductGuid");

                    b.HasIndex("ProductPriceGuid");

                    b.ToTable("lm_invoice_sale_product");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.SystemManagement.Notification", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("body");

                    b.Property<string>("Heading")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("heading");

                    b.Property<bool?>("IsSystemWide")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_system_wide");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("sent_at");

                    b.HasKey("Guid");

                    b.ToTable("sm_notification");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.SystemManagement.NotificationUser", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool?>("HaveRead")
                        .IsRequired()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("have_read");

                    b.Property<Guid>("NotificationGuid")
                        .HasColumnType("char(36)")
                        .HasColumnName("notification_guid");

                    b.Property<Guid?>("UserGuid")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("user_guid");

                    b.HasKey("Guid");

                    b.HasIndex("NotificationGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("sm_notification_user");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.SystemManagement.Setting", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AddressOne")
                        .HasColumnType("varchar(256)")
                        .HasColumnName("address_one");

                    b.Property<string>("AddressTwo")
                        .HasColumnType("varchar(256)")
                        .HasColumnName("address_two");

                    b.Property<int>("DefaultProfitPercent")
                        .HasColumnType("int")
                        .HasColumnName("default_profit_percent");

                    b.Property<int>("DefaultProfitPercentForWholeSale")
                        .HasColumnType("int")
                        .HasColumnName("default_profit_percent_for_whole_sale");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(256)")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .HasColumnType("longtext")
                        .HasColumnName("image");

                    b.Property<int>("MinimumStockMargin")
                        .HasColumnType("int")
                        .HasColumnName("minimum_stock_margin");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("name");

                    b.Property<string>("PhoneFour")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("phone_four");

                    b.Property<string>("PhoneOne")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("phone_one");

                    b.Property<string>("PhoneThree")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("phone_three");

                    b.Property<string>("PhoneTwo")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("phone_two");

                    b.Property<bool>("SuggestSalePrice")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("suggest_sale_price");

                    b.HasKey("Guid");

                    b.ToTable("sm_setting");

                    b.HasData(
                        new
                        {
                            Guid = new Guid("b1819545-6b5b-4114-8b66-76065037b1de"),
                            AddressOne = "အမှတ်(၃၁), တိုးချဲ့ကမ်းနားလမ်း, မြိတ်တံတားထိပ်, ဖက်တန်းရပ်, မော်လမြိုင်မြို့",
                            AddressTwo = "အမှတ်(၁၈), ကျိုက်သန်လန် ဘုရားလမ်း, ရွှေတောင်ရပ်, မော်လမြိုင်မြို့",
                            DefaultProfitPercent = 15,
                            DefaultProfitPercentForWholeSale = 10,
                            Description = "သီဟိုဠ်ဆံ နှင့် မုန့်မျိုးစုံရောင်းဝယ်ရေး",
                            Image = "logo.png",
                            MinimumStockMargin = 10,
                            Name = "MK Trading",
                            PhoneFour = "057-24258",
                            PhoneOne = "09-446639594",
                            PhoneThree = "09-760142884",
                            PhoneTwo = "09-777308660",
                            SuggestSalePrice = true
                        });
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.User", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.AccountManagement.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.UserLogin", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.AccountManagement.User", "User")
                        .WithOne("UserLogin")
                        .HasForeignKey("Friday.ERP.Core.Data.Entities.AccountManagement.UserLogin", "UserGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.Product", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.InventoryManagement.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.ProductPrice", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.InventoryManagement.Product", "Product")
                        .WithMany("ProductPrices")
                        .HasForeignKey("ProductGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoicePurchase", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.CustomerVendorManagement.CustomerVendor", "Vendor")
                        .WithMany("InvoicePurchases")
                        .HasForeignKey("VendorGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoicePurchaseProduct", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoicePurchase", "InvoicePurchase")
                        .WithMany("PurchasedProducts")
                        .HasForeignKey("InvoicePurchaseGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Friday.ERP.Core.Data.Entities.InventoryManagement.Product", "Product")
                        .WithMany("InvoicePurchaseProducts")
                        .HasForeignKey("ProductGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvoicePurchase");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSale", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.CustomerVendorManagement.CustomerVendor", "Customer")
                        .WithMany("InvoiceSales")
                        .HasForeignKey("CustomerGuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSaleDelivery", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSale", "InvoiceSale")
                        .WithOne("InvoiceSaleDelivery")
                        .HasForeignKey("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSaleDelivery", "InvoiceSaleGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvoiceSale");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSaleProduct", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSale", "InvoiceSale")
                        .WithMany("SoldProducts")
                        .HasForeignKey("InvoiceSaleGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Friday.ERP.Core.Data.Entities.InventoryManagement.Product", "Product")
                        .WithMany("InvoiceSaleProducts")
                        .HasForeignKey("ProductGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Friday.ERP.Core.Data.Entities.InventoryManagement.ProductPrice", "ProductPrice")
                        .WithMany("InvoiceProduct")
                        .HasForeignKey("ProductPriceGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvoiceSale");

                    b.Navigation("Product");

                    b.Navigation("ProductPrice");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.SystemManagement.NotificationUser", b =>
                {
                    b.HasOne("Friday.ERP.Core.Data.Entities.SystemManagement.Notification", "Notification")
                        .WithMany("NotificationUsers")
                        .HasForeignKey("NotificationGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Friday.ERP.Core.Data.Entities.AccountManagement.User", "User")
                        .WithMany("NotificationUsers")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Notification");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.User", b =>
                {
                    b.Navigation("NotificationUsers");

                    b.Navigation("UserLogin");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.AccountManagement.UserRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.CustomerVendorManagement.CustomerVendor", b =>
                {
                    b.Navigation("InvoicePurchases");

                    b.Navigation("InvoiceSales");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.Product", b =>
                {
                    b.Navigation("InvoicePurchaseProducts");

                    b.Navigation("InvoiceSaleProducts");

                    b.Navigation("ProductPrices");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InventoryManagement.ProductPrice", b =>
                {
                    b.Navigation("InvoiceProduct");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoicePurchase", b =>
                {
                    b.Navigation("PurchasedProducts");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.InvoiceManagement.InvoiceSale", b =>
                {
                    b.Navigation("InvoiceSaleDelivery");

                    b.Navigation("SoldProducts");
                });

            modelBuilder.Entity("Friday.ERP.Core.Data.Entities.SystemManagement.Notification", b =>
                {
                    b.Navigation("NotificationUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
