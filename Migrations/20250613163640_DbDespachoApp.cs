using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWebDespachos.Migrations
{
    /// <inheritdoc />
    public partial class DbDespachoApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_rol = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    User = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_Id_rol",
                        column: x => x.Id_rol,
                        principalTable: "Roles",
                        principalColumn: "Id_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Razon_social = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cuit = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    UsuarioId_usuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id_cliente);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_UsuarioId_usuario",
                        column: x => x.UsuarioId_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Precio_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id_producto);
                    table.ForeignKey(
                        name: "FK_Productos_Usuarios_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Direcciones",
                columns: table => new
                {
                    Id_direccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_cliente = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nro = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Piso = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Dpto = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cp = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsuarioId_usuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.Id_direccion);
                    table.ForeignKey(
                        name: "FK_Direcciones_Clientes_Id_cliente",
                        column: x => x.Id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "Id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Direcciones_Usuarios_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Direcciones_Usuarios_UsuarioId_usuario",
                        column: x => x.UsuarioId_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id_pedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_cliente = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cuit = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id_pedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_Id_cliente",
                        column: x => x.Id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "Id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallePedidos",
                columns: table => new
                {
                    Nro_renglon = table.Column<int>(type: "int", nullable: false),
                    Id_pedido = table.Column<int>(type: "int", nullable: false),
                    Id_producto = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Precio_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductoId_producto = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePedidos", x => new { x.Id_pedido, x.Nro_renglon });
                    table.ForeignKey(
                        name: "FK_DetallePedidos_Pedidos_Id_pedido",
                        column: x => x.Id_pedido,
                        principalTable: "Pedidos",
                        principalColumn: "Id_pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallePedidos_Productos_Id_producto",
                        column: x => x.Id_producto,
                        principalTable: "Productos",
                        principalColumn: "Id_producto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallePedidos_Productos_ProductoId_producto",
                        column: x => x.ProductoId_producto,
                        principalTable: "Productos",
                        principalColumn: "Id_producto");
                    table.ForeignKey(
                        name: "FK_DetallePedidos_Usuarios_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Id_usuario",
                table: "Clientes",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId_usuario",
                table: "Clientes",
                column: "UsuarioId_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_Id_producto",
                table: "DetallePedidos",
                column: "Id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_Id_usuario",
                table: "DetallePedidos",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_ProductoId_producto",
                table: "DetallePedidos",
                column: "ProductoId_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_Id_cliente",
                table: "Direcciones",
                column: "Id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_Id_usuario",
                table: "Direcciones",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_UsuarioId_usuario",
                table: "Direcciones",
                column: "UsuarioId_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Id_cliente",
                table: "Pedidos",
                column: "Id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Id_usuario",
                table: "Pedidos",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Id_usuario",
                table: "Productos",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Id_rol",
                table: "Usuarios",
                column: "Id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallePedidos");

            migrationBuilder.DropTable(
                name: "Direcciones");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
