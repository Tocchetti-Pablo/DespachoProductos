using Microsoft.EntityFrameworkCore;
using AppWebDespachos.Models; 

namespace AppWebDespachos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
             : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave compuesta para DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasKey(d => new { d.Id_pedido, d.Nro_renglon });

            // Evitar cascada múltiple desde Direccion hacia Usuario
            modelBuilder.Entity<Direccion>()
                .HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.Id_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Si Cliente también tiene FK a Usuario evita cascada múltiple
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.Id_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.Id_usuario)
                .OnDelete(DeleteBehavior.Restrict); // o .NoAction()

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.Id_producto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.Id_pedido)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio_unitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.Precio_unitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.Subtotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(18, 2);

            //Crea los roles
            modelBuilder.Entity<Rol>()
                .HasData(
                 new Rol { Id_rol = 1, Nombre = "Administrador", Descripcion = "Acceso total", Habilitado = true },
                 new Rol { Id_rol = 2, Nombre = "Usuario", Descripcion = "Acceso limitado", Habilitado = true }
                );

            modelBuilder.Entity<Usuario>()
                .HasData(
                    new Usuario
                    {
                        Id_usuario = 1,
                        Id_rol = 1, // Administrador
                        Nombre = "Admin",
                        Apellido = "Principal",
                        Email = "admin@tusistema.com",
                        User = "admin",
                        Password = "admin123", 
                        Habilitado = true
                    }
                );

        }
    }
}

