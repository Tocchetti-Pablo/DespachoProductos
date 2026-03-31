namespace AppWebDespachos.ViewModels
{
    public class ProductoCantidadViewModel
    {
        public int Id_producto { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio_unitario { get; set; }
        public int Cantidad { get; set; } // cantidad actual editable
    }

}
