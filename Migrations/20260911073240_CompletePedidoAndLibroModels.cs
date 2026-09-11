using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookfyApi.Migrations
{
    /// <inheritdoc />
    public partial class CompletePedidoAndLibroModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Pedidos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedidos");
        }
    }
}
