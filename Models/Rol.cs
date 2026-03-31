using System.ComponentModel.DataAnnotations;

namespace AppWebDespachos.Models
{
    public class Rol
    {
        [Key]
        public int Id_rol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(30)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        public bool Habilitado { get; set; }

        public Rol() { } //Constructor vacio 

        // Relación: 1 Rol → N Usuarios
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    }
}
