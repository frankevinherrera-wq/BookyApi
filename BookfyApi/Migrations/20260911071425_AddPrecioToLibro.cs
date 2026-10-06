using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookfyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPrecioToLibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Libros",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Libros");
        }
    }
}
