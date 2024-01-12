using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friday.ERP.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "am_user_role",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_am_user_role", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "cm_customer_vendor",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    code = table.Column<string>(type: "varchar(256)", nullable: false),
                    name = table.Column<string>(type: "varchar(256)", nullable: false),
                    phone = table.Column<string>(type: "varchar(15)", nullable: false),
                    email = table.Column<string>(type: "varchar(50)", nullable: true),
                    address = table.Column<string>(type: "varchar(50)", nullable: true),
                    customer_vendor_type = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    total_credit_debit_left = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cm_customer_vendor", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "im_category",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    name = table.Column<string>(type: "varchar(256)", nullable: false),
                    color = table.Column<string>(type: "varchar(256)", nullable: false),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_im_category", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "sm_notification",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    heading = table.Column<string>(type: "varchar(255)", nullable: false),
                    body = table.Column<string>(type: "varchar(255)", nullable: false),
                    is_system_wide = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    sent_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm_notification", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "sm_setting",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    image = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    name = table.Column<string>(type: "varchar(256)", nullable: false),
                    description = table.Column<string>(type: "varchar(256)", nullable: true),
                    address_one = table.Column<string>(type: "varchar(256)", nullable: true),
                    address_two = table.Column<string>(type: "varchar(256)", nullable: true),
                    phone_one = table.Column<string>(type: "varchar(100)", nullable: true),
                    phone_two = table.Column<string>(type: "varchar(100)", nullable: true),
                    phone_three = table.Column<string>(type: "varchar(100)", nullable: true),
                    phone_four = table.Column<string>(type: "varchar(100)", nullable: true),
                    suggest_sale_price = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    default_profit_percent = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    default_profit_percent_for_whole_sale = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    minimum_stock_margin = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm_setting", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "am_user",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    phone = table.Column<string>(type: "varchar(50)", nullable: true),
                    email = table.Column<string>(type: "varchar(50)", nullable: true),
                    username = table.Column<string>(type: "varchar(10)", nullable: false),
                    password = table.Column<string>(type: "varchar(100)", nullable: false),
                    user_role_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_am_user", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_am_user_am_user_role_user_role_guid",
                        column: x => x.user_role_guid,
                        principalTable: "am_user_role",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lm_invoice_purchase",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    invoice_no = table.Column<string>(type: "varchar(50)", nullable: false),
                    sub_total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    discount = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    discount_type = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    delivery_fees = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    grand_total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    paid_total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    remark = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    credit_debit_left = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    is_paid = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    vendor_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lm_invoice_purchase", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_lm_invoice_purchase_cm_customer_vendor_vendor_guid",
                        column: x => x.vendor_guid,
                        principalTable: "cm_customer_vendor",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lm_invoice_sale",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    invoice_no = table.Column<string>(type: "varchar(50)", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    sub_total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    discount = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    discount_type = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    delivery_fees = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    grand_total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    paid_total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    remark = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    credit_debit_left = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    customer_guid = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lm_invoice_sale", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_lm_invoice_sale_cm_customer_vendor_customer_guid",
                        column: x => x.customer_guid,
                        principalTable: "cm_customer_vendor",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "im_product",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    code = table.Column<string>(type: "varchar(256)", nullable: false),
                    image = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    name = table.Column<string>(type: "varchar(256)", nullable: false),
                    stock = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    category_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_im_product", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_im_product_im_category_category_guid",
                        column: x => x.category_guid,
                        principalTable: "im_category",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "am_user_login",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    access_token = table.Column<string>(type: "varchar(512)", nullable: false),
                    refresh_token = table.Column<string>(type: "varchar(512)", nullable: false),
                    refresh_token_expiry_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    user_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_am_user_login", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_am_user_login_am_user_user_guid",
                        column: x => x.user_guid,
                        principalTable: "am_user",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sm_notification_user",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    have_read = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    notification_guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    user_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm_notification_user", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_sm_notification_user_am_user_user_guid",
                        column: x => x.user_guid,
                        principalTable: "am_user",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm_notification_user_sm_notification_notification_guid",
                        column: x => x.notification_guid,
                        principalTable: "sm_notification",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lm_invoice_sale_delivery",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    delivery_service_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    delivery_contact_person = table.Column<string>(type: "varchar(50)", nullable: true),
                    delivery_contact_phone = table.Column<string>(type: "varchar(50)", nullable: true),
                    remark = table.Column<string>(type: "varchar(50)", nullable: true),
                    invoice_sale_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lm_invoice_sale_delivery", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_lm_invoice_sale_delivery_lm_invoice_sale_invoice_sale_guid",
                        column: x => x.invoice_sale_guid,
                        principalTable: "lm_invoice_sale",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "im_product_price",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    sale_price = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    is_whole_sale = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    action_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    product_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_im_product_price", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_im_product_price_im_product_product_guid",
                        column: x => x.product_guid,
                        principalTable: "im_product",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lm_invoice_purchase_product",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    quantity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    purchased_price = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    product_guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    invoice_purchase_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lm_invoice_purchase_product", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_lm_invoice_purchase_product_im_product_product_guid",
                        column: x => x.product_guid,
                        principalTable: "im_product",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lm_invoice_purchase_product_lm_invoice_purchase_invoice_purchase_guid",
                        column: x => x.invoice_purchase_guid,
                        principalTable: "lm_invoice_purchase",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lm_invoice_sale_product",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    quantity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    total = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    product_guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    invoice_sale_guid = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    product_price_guid = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lm_invoice_sale_product", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_lm_invoice_sale_product_im_product_price_product_price_guid",
                        column: x => x.product_price_guid,
                        principalTable: "im_product_price",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lm_invoice_sale_product_im_product_product_guid",
                        column: x => x.product_guid,
                        principalTable: "im_product",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lm_invoice_sale_product_lm_invoice_sale_invoice_sale_guid",
                        column: x => x.invoice_sale_guid,
                        principalTable: "lm_invoice_sale",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "am_user_role",
                columns: new[] { "Guid", "name" },
                values: new object[] { new Guid("7d5ac91f-48ab-4ec7-883e-ba83a13e26bb"), "Admin" });

            migrationBuilder.InsertData(
                table: "sm_setting",
                columns: new[] { "Guid", "address_one", "address_two", "default_profit_percent", "default_profit_percent_for_whole_sale", "description", "image", "minimum_stock_margin", "name", "phone_four", "phone_one", "phone_three", "phone_two", "suggest_sale_price" },
                values: new object[] { new Guid("b1819545-6b5b-4114-8b66-76065037b1de"), "အမှတ်(၃၁), တိုးချဲ့ကမ်းနားလမ်း, မြိတ်တံတားထိပ်, ဖက်တန်းရပ်, မော်လမြိုင်မြို့", "အမှတ်(၁၈), ကျိုက်သန်လန် ဘုရားလမ်း, ရွှေတောင်ရပ်, မော်လမြိုင်မြို့", 15, 10, "သီဟိုဠ်ဆံ နှင့် မုန့်မျိုးစုံရောင်းဝယ်ရေး", "logo.png", 10, "MK Trading", "057-24258", "09-446639594", "09-760142884", "09-777308660", true });

            migrationBuilder.InsertData(
                table: "am_user",
                columns: new[] { "Guid", "email", "name", "password", "phone", "user_role_guid", "username" },
                values: new object[] { new Guid("55b36e77-3ac8-46d8-9611-d1987d8041be"), "Admin@admin.com", "Admin", "$2a$11$BDyDgdnLAS9iFc2K/kR.R.RvOoKWCVOExGk708Qa5FlsO6Sn9x7ze", null, new Guid("7d5ac91f-48ab-4ec7-883e-ba83a13e26bb"), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_am_user_email",
                table: "am_user",
                column: "email",
                unique: true,
                filter: "\"email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_am_user_name",
                table: "am_user",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_am_user_phone",
                table: "am_user",
                column: "phone",
                unique: true,
                filter: "\"phone\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_am_user_user_role_guid",
                table: "am_user",
                column: "user_role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_am_user_username",
                table: "am_user",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_am_user_login_access_token",
                table: "am_user_login",
                column: "access_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_am_user_login_refresh_token",
                table: "am_user_login",
                column: "refresh_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_am_user_login_user_guid",
                table: "am_user_login",
                column: "user_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_am_user_role_name",
                table: "am_user_role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cm_customer_vendor_code",
                table: "cm_customer_vendor",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cm_customer_vendor_email",
                table: "cm_customer_vendor",
                column: "email",
                unique: true,
                filter: "\"email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_cm_customer_vendor_phone",
                table: "cm_customer_vendor",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_im_category_name",
                table: "im_category",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_im_product_category_guid",
                table: "im_product",
                column: "category_guid");

            migrationBuilder.CreateIndex(
                name: "IX_im_product_code",
                table: "im_product",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_im_product_name",
                table: "im_product",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_im_product_price_product_guid",
                table: "im_product_price",
                column: "product_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_purchase_vendor_guid",
                table: "lm_invoice_purchase",
                column: "vendor_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_purchase_product_invoice_purchase_guid",
                table: "lm_invoice_purchase_product",
                column: "invoice_purchase_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_purchase_product_product_guid",
                table: "lm_invoice_purchase_product",
                column: "product_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_sale_customer_guid",
                table: "lm_invoice_sale",
                column: "customer_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_sale_delivery_invoice_sale_guid",
                table: "lm_invoice_sale_delivery",
                column: "invoice_sale_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_sale_product_invoice_sale_guid",
                table: "lm_invoice_sale_product",
                column: "invoice_sale_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_sale_product_product_guid",
                table: "lm_invoice_sale_product",
                column: "product_guid");

            migrationBuilder.CreateIndex(
                name: "IX_lm_invoice_sale_product_product_price_guid",
                table: "lm_invoice_sale_product",
                column: "product_price_guid");

            migrationBuilder.CreateIndex(
                name: "IX_sm_notification_user_notification_guid",
                table: "sm_notification_user",
                column: "notification_guid");

            migrationBuilder.CreateIndex(
                name: "IX_sm_notification_user_user_guid",
                table: "sm_notification_user",
                column: "user_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "am_user_login");

            migrationBuilder.DropTable(
                name: "lm_invoice_purchase_product");

            migrationBuilder.DropTable(
                name: "lm_invoice_sale_delivery");

            migrationBuilder.DropTable(
                name: "lm_invoice_sale_product");

            migrationBuilder.DropTable(
                name: "sm_notification_user");

            migrationBuilder.DropTable(
                name: "sm_setting");

            migrationBuilder.DropTable(
                name: "lm_invoice_purchase");

            migrationBuilder.DropTable(
                name: "im_product_price");

            migrationBuilder.DropTable(
                name: "lm_invoice_sale");

            migrationBuilder.DropTable(
                name: "am_user");

            migrationBuilder.DropTable(
                name: "sm_notification");

            migrationBuilder.DropTable(
                name: "im_product");

            migrationBuilder.DropTable(
                name: "cm_customer_vendor");

            migrationBuilder.DropTable(
                name: "am_user_role");

            migrationBuilder.DropTable(
                name: "im_category");
        }
    }
}
