using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppWebDespachos.Models
{
    public class Direccion
    {
        [Key]
        public int Id_direccion { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int Id_cliente { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int Id_usuario { get; set; }

        [Required(ErrorMessage = "La calle es obligatoria.")]
        [StringLength(50)]
        public string Calle { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número es obligatorio.")]
        [StringLength(10)]
        public string Nro { get; set; } = string.Empty;

        [StringLength(5)]
        public string Piso { get; set; } = string.Empty;

        [StringLength(5)]
        public string Dpto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(50)]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código postal es obligatorio.")]
        [StringLength(10)]
        public string Cp { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        [StringLength(50)]
        public string Provincia { get; set; } = string.Empty;

        public Direccion() { }

        // Relaciones de navegación
        [ForeignKey("Id_cliente")]
        [ValidateNever]
        public Cliente Cliente { get; set; } = null!;

        [ForeignKey("Id_usuario")]
        [ValidateNever]
        public Usuario Usuario { get; set; } = null!;
    }
}

