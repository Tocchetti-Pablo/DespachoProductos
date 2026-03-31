using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppWebDespachos.Models
{
    public class Cliente
    {
        [Key]
        public int Id_cliente { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(100)]

        [Display(Name = "Razón social")]
        public string Razon_social { get; set; } = string.Empty;

        [StringLength(150)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El CUIT es obligatorio.")]
        [StringLength(13, MinimumLength = 11, ErrorMessage = "El CUIT debe tener entre 11 y 13 caracteres.")]
        public string Cuit { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        // Foreign key - usuario que cargó el cliente
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int Id_usuario { get; set; }

        [ForeignKey("Id_usuario")]
        [ValidateNever]
        public Usuario Usuario { get; set; } = null!;

        public bool Habilitado { get; set; } = true;
        public Cliente() { } //Constructor vacio 

        // Relaciones
        public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
