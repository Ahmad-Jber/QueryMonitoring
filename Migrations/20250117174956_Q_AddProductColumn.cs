using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueryMonitoring.Migrations
{
    /// <inheritdoc />
    public partial class Q_AddProductColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Database",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Database",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Database",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Database",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Database",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Database",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "Database",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Database",
                table: "Attachments");
        }
    }
}
