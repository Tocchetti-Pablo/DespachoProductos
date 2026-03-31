using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppWebDespachos.Models
{
    public class Usuario
    {
        [Key]
        public int Id_usuario { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int Id_rol { get; set; } // FK: Rol

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(30)]
        public string User { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        public bool Habilitado { get; set; }

        public Usuario() { } // Constructor vacío

        // Relación con Rol
        [ForeignKey("Id_rol")]
        [ValidateNever]
        public Rol Rol { get; set; } = null!;

        // Relaciones hacia otras entidades
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
        //public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
        //public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();
    }
}
