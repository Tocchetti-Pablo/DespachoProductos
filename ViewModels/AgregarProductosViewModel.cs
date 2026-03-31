using AppWebDespachos.Models;
using System.ComponentModel.DataAnnotations;

namespace AppWebDespachos.ViewModels
{
    public class ProductoCantidadViewModel
    {
        public int Id_producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio_unitario { get; set; }
        public int Cantidad { get; set; }
    }

    public class AgregarProductosViewModel
    {
        public int Id_pedido { get; set; }
        public List<ProductoCantidadViewModel> Productos { get; set; } = new List<ProductoCantidadViewModel>();
    }
    
    public class EditarDetallePedidoViewModel
    {
        public int Id_pedido { get; set; }
        public List<ProductoCantidadViewModel> ProductosDisponibles { get; set; } = new();
    }
}