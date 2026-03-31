using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppWebDespachos.Models
{
    public class Pedido
    {
        [Key]
        public int Id_pedido { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int Id_cliente { get; set; } // FK: Cliente

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int Id_usuario { get; set; } // FK: Usuario

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [StringLength(13)]
        public string Cuit { get; set; } = string.Empty;

        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]// Total puede ser 0 al inicio
        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        public Pedido() { } // Constructor vacío

        public virtual ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

        // Relaciones de navegación con ForeignKey
        [ForeignKey("Id_cliente")]
        [ValidateNever]
        public Cliente Cliente { get; set; } = null!;

        [ForeignKey("Id_usuario")]
        [ValidateNever]
        public Usuario Usuario { get; set; } = null!;
    }
}
