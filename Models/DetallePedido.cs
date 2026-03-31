using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppWebDespachos.Models
{
    public class DetallePedido
    {
        // Clave primaria compuesta definida en DbContext (Fluent API)
        public int Nro_renglon { get; set; }

        [Required(ErrorMessage = "El pedido es obligatorio.")]
        public int Id_pedido { get; set; } // FK: Pedido

        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int Id_producto { get; set; } // FK: Producto

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int Id_usuario { get; set; } // FK: Usuario

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        [StringLength(150)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio_unitario { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a cero.")]
        public decimal Subtotal { get; set; }

        public DetallePedido() { } // Constructor vacío 

        // Relaciones y FK
        [ForeignKey("Id_pedido")]
        [ValidateNever]
        public Pedido? Pedido { get; set; }

        [ForeignKey("Id_producto")]
        [ValidateNever]
        public Producto? Producto { get; set; }

        [ForeignKey("Id_usuario")]
        [ValidateNever]
        public Usuario? Usuario { get; set; }
    }
}
