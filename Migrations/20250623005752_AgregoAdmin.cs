using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWebDespachos.Migrations
{
    /// <inheritdoc />
    public partial class AgregoAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id_usuario", "Apellido", "Email", "Habilitado", "Id_rol", "Nombre", "Password", "User" },
                values: new object[] { 1, "Principal", "admin@tusistema.com", true, 1, "Admin", "admin123", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id_usuario",
                keyValue: 1);
        }
    }
}
