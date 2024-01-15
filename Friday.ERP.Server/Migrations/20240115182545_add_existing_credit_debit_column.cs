using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friday.ERP.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_existing_credit_debit_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "existing_credit_debit",
                table: "lm_invoice_sale",
                type: "NUMBER(19)",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "existing_credit_debit",
                table: "lm_invoice_purchase",
                type: "NUMBER(19)",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "existing_credit_debit",
                table: "lm_invoice_sale");

            migrationBuilder.DropColumn(
                name: "existing_credit_debit",
                table: "lm_invoice_purchase");
        }
    }
}
