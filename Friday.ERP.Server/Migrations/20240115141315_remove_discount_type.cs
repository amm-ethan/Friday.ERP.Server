using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friday.ERP.Server.Migrations
{
    /// <inheritdoc />
    public partial class remove_discount_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_type",
                table: "lm_invoice_sale");

            migrationBuilder.DropColumn(
                name: "discount_type",
                table: "lm_invoice_purchase");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "discount_type",
                table: "lm_invoice_sale",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "discount_type",
                table: "lm_invoice_purchase",
                type: "NUMBER(10)",
                nullable: true);
        }
    }
}
