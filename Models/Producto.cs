using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppWebDespachos.Models
{
    public class Producto
    {
        [Key]
        public int Id_producto { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int Id_usuario { get; set; } // FK: Usuario

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(150)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio_unitario { get; set; }

        public bool Habilitado { get; set; }

        public Producto() { } // Constructor vacío

        [ForeignKey("Id_usuario")]
        [ValidateNever]
        public Usuario Usuario { get; set; } = null!;

        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();

    }
}
