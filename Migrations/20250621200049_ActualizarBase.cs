using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWebDespachos.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Usuarios_UsuarioId_usuario",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Direcciones_Usuarios_UsuarioId_usuario",
                table: "Direcciones");

            migrationBuilder.DropIndex(
                name: "IX_Direcciones_UsuarioId_usuario",
                table: "Direcciones");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_UsuarioId_usuario",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UsuarioId_usuario",
                table: "Direcciones");

            migrationBuilder.DropColumn(
                name: "UsuarioId_usuario",
                table: "Clientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId_usuario",
                table: "Direcciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId_usuario",
                table: "Clientes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_UsuarioId_usuario",
                table: "Direcciones",
                column: "UsuarioId_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId_usuario",
                table: "Clientes",
                column: "UsuarioId_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Usuarios_UsuarioId_usuario",
                table: "Clientes",
                column: "UsuarioId_usuario",
                principalTable: "Usuarios",
                principalColumn: "Id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Direcciones_Usuarios_UsuarioId_usuario",
                table: "Direcciones",
                column: "UsuarioId_usuario",
                principalTable: "Usuarios",
                principalColumn: "Id_usuario");
        }
    }
}
