using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppWebDespachos.Data;
using AppWebDespachos.Models;

namespace AppWebDespachos.Controllers
{
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index(string busquedaUsuario)
        {
            var pedidos = _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(busquedaUsuario))
            {
                pedidos = pedidos.Where(p =>
                    p.Usuario.Nombre.Contains(busquedaUsuario) ||
                    p.Usuario.Apellido.Contains(busquedaUsuario));
            }

            return View(await pedidos.ToListAsync());
        }


        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id_pedido == id);

            if (pedido == null)
                return NotFound();

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewBag.Id_cliente = new SelectList(_context.Clientes, "Id_cliente", "Razon_social");

            var claim = User.FindFirst("Id_usuario");
            if (claim == null || !int.TryParse(claim.Value, out var idUsuario))
            {
                return Forbid(); // o redirigí al login si querés
            }
            

            var usuario = _context.Usuarios
                .Where(u => u.Id_usuario == idUsuario)
                .Select(u => new { u.Id_usuario, Nombre = u.Apellido + ", " + u.Nombre })
                .ToList();

            ViewBag.Id_usuario = new SelectList(usuario, "Id_usuario", "Nombre", idUsuario);

            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_cliente")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                // Obtener ID del usuario logueado desde la cookie o Claims
                var claim = User.FindFirst("Id_usuario");
                if (claim == null || !int.TryParse(claim.Value, out var idUsuario))
                {
                    return Forbid(); // o redirigí al login si querés
                }
                pedido.Id_usuario = idUsuario;

                var cliente = await _context.Clientes.FindAsync(pedido.Id_cliente);
                if (cliente != null)
                {
                    pedido.Estado = "Pendiente";
                    pedido.Cuit = cliente.Cuit;
                    pedido.Nombre = cliente.Razon_social;
                }

                pedido.Total = 0;
                pedido.Fecha = DateTime.Now;

                _context.Add(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create", "DetallePedidos", new { idPedido = pedido.Id_pedido });
            }

            ViewBag.Id_cliente = new SelectList(_context.Clientes, "Id_cliente", "Razon_social", pedido.Id_cliente);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id_pedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles) // Incluye los detalles del pedido
                .FirstOrDefaultAsync(p => p.Id_pedido == id);

            if (pedido == null)
                return NotFound();

            if (pedido.Estado != "Pendiente")
            {
                TempData["ErrorMessage"] = "Solo se pueden eliminar pedidos en estado 'Pendiente'.";
                return RedirectToAction(nameof(Index));
            }

            // Reintegrar stock
            foreach (var detalle in pedido.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.Id_producto);
                if (producto != null)
                {
                    producto.Stock += detalle.Cantidad;
                    _context.Update(producto);
                }
            }

            // Eliminar detalles y pedido
            _context.DetallePedidos.RemoveRange(pedido.Detalles);
            _context.Pedidos.Remove(pedido);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Pedido eliminado y stock restaurado correctamente.";
            return RedirectToAction(nameof(Index));
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmar(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            pedido.Estado = "Confirmado";
            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id_pedido == id);
        }
    }
}
