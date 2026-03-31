using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWebDespachos.Migrations
{
    /// <inheritdoc />
    public partial class PropiedadBoolClienteHabilitado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Habilitado",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Habilitado",
                table: "Clientes");
        }
    }
}
