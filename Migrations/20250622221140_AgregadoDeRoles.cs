using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWebDespachos.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoDeRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id_rol", "Descripcion", "Habilitado", "Nombre" },
                values: new object[,]
                {
                    { 1, "Acceso total", true, "Administrador" },
                    { 2, "Acceso limitado", true, "Usuario" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id_rol",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id_rol",
                keyValue: 2);
        }
    }
}
