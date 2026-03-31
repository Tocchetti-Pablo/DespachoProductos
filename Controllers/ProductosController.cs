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
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Productos.Include(p => p.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id_producto == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["Id_usuario"] = new SelectList(_context.Usuarios.Where(u => u.Habilitado), "Id_usuario", "Apellido");
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                // Obtener ID del usuario logueado desde la cookie o Claims
                var claim = User.FindFirst("Id_usuario");
                if (claim == null || !int.TryParse(claim.Value, out var idUsuario))
                {
                    return Forbid(); // Seguridad: usuario no autenticado o claim ausente
                }

                producto.Id_usuario = idUsuario;

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            ViewData["Id_usuario"] = new SelectList(_context.Usuarios.Where(u => u.Habilitado), "Id_usuario", "Apellido", producto.Id_usuario);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_producto,Nombre,Descripcion,Stock,Precio_unitario")] Producto producto)
        {
            if (id != producto.Id_producto) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var productoExistente = await _context.Productos.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id_producto == id);

                    if (productoExistente == null) return NotFound();

                    // Obtener el Id_usuario desde el claim del usuario logueado
                    var claim = User.FindFirst("Id_usuario");
                    if (claim == null)
                    {
                        ModelState.AddModelError("", "No se pudo obtener el usuario de sesión.");
                        return View(producto);
                    }

                    int idUsuario = int.Parse(claim.Value);

                    // Asignar el usuario actual
                    producto.Id_usuario = idUsuario;

                    // Mantener el valor de Habilitado original
                    producto.Habilitado = productoExistente.Habilitado;

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id_producto)) return NotFound();
                    else throw;
                }
            }

            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id_producto == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null) return NotFound();

            productoExistente.Habilitado = !productoExistente.Habilitado;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id_producto == id);
        }
    }
}
