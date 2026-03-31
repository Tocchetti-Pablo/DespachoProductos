using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppWebDespachos.Data;
using AppWebDespachos.Models;
using AppWebDespachos.ViewModels;

namespace AppWebDespachos.Controllers
{
    public class DetallePedidosController : Controller
    {
        private readonly AppDbContext _context;

        public DetallePedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DetallePedidos/Create
        [HttpGet]
        public async Task<IActionResult> Create(int idPedido, string? searchQuery)
        {
            // Obtener todos los productos habilitados y con stock > 0
            var productosDisponibles = await _context.Productos
                .Where(p => p.Habilitado && p.Stock > 0)
                .Select(p => new ProductoCantidadViewModel
                {
                    Id_producto = p.Id_producto,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio_unitario = p.Precio_unitario,
                    Cantidad = 0
                }).ToListAsync();

            // Si se hizo una búsqueda por ID, buscar ese producto específico
            if (!string.IsNullOrEmpty(searchQuery) && int.TryParse(searchQuery, out int idProductoBuscado))
            {
                var productoEncontrado = productosDisponibles.FirstOrDefault(p => p.Id_producto == idProductoBuscado);

                if (productoEncontrado != null)
                {
                    // Reordenar: poner el producto buscado al principio
                    productosDisponibles = productosDisponibles
                        .Where(p => p.Id_producto != idProductoBuscado)
                        .Prepend(productoEncontrado)
                        .ToList();

                    ViewBag.CurrentSearchQuery = searchQuery;
                }
                else
                {
                    ModelState.AddModelError("", "No se encontró un producto con ese ID o no tiene stock.");
                }
            }

            var viewModel = new AgregarProductosViewModel
            {
                Id_pedido = idPedido,
                Productos = productosDisponibles
            };

            return View(viewModel);
        }


        public async Task<IActionResult> EditarDetalle(int idPedido)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .ThenInclude(dp => dp.Producto)
                .FirstOrDefaultAsync(p => p.Id_pedido == idPedido);

            if (pedido == null)
                return NotFound();

            var productos = await _context.Productos
                .Where(p => p.Habilitado && p.Stock > 0)
                .ToListAsync();

            var viewModel = new EditarDetallePedidoViewModel
            {
                Id_pedido = pedido.Id_pedido,
                ProductosDisponibles = productos.Select(p => {
                    var detalle = pedido.Detalles.FirstOrDefault(dp => dp.Id_producto == p.Id_producto);
                    return new ProductoCantidadViewModel
                    {
                        Id_producto = p.Id_producto,
                        Nombre = p.Nombre,
                        Precio_unitario = p.Precio_unitario,
                        Cantidad = detalle?.Cantidad ?? 0
                    };
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: DetallePedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgregarProductosViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id_pedido == model.Id_pedido);

            if (pedido == null || pedido.Estado != "Pendiente")
            {
                ModelState.AddModelError("", "No se puede modificar este pedido.");
                return View(model);
            }

            int nroRenglon = pedido.Detalles.Count + 1;
            var detallesNuevos = new List<DetallePedido>();

            foreach (var p in model.Productos.Where(p => p.Cantidad > 0))
            {
                // Validar si ya está agregado
                bool yaExiste = pedido.Detalles.Any(d => d.Id_producto == p.Id_producto);
                if (yaExiste)
                {
                    ModelState.AddModelError("", $"El producto '{p.Nombre}' ya está agregado al pedido.");
                    continue;
                }

                var producto = await _context.Productos.FindAsync(p.Id_producto);
                if (producto == null)
                {
                    ModelState.AddModelError("", $"Producto con ID {p.Id_producto} no encontrado.");
                    continue;
                }

                if (p.Cantidad > producto.Stock)
                {
                    ModelState.AddModelError("", $"Stock insuficiente para '{producto.Nombre}'. Stock disponible: {producto.Stock}, solicitado: {p.Cantidad}.");
                    continue;
                }

                producto.Stock -= p.Cantidad;

                detallesNuevos.Add(new DetallePedido
                {
                    Id_pedido = model.Id_pedido,
                    Nro_renglon = nroRenglon++,
                    Id_producto = p.Id_producto,
                    Cantidad = p.Cantidad,
                    Precio_unitario = p.Precio_unitario,
                    Subtotal = p.Precio_unitario * p.Cantidad,
                    Id_usuario = pedido.Id_usuario
                });

                _context.Update(producto);
            }

            if (detallesNuevos.Any())
            {
                _context.DetallePedidos.AddRange(detallesNuevos);
                pedido.Total += detallesNuevos.Sum(d => d.Subtotal);
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }

            if (!ModelState.IsValid)
            {
                // Recargar productos si hubo errores para que la vista no rompa
                model.Productos = await _context.Productos
                    .Where(p => p.Habilitado && p.Stock > 0)
                    .Select(p => new ProductoCantidadViewModel
                    {
                        Id_producto = p.Id_producto,
                        Nombre = p.Nombre,
                        Precio_unitario = p.Precio_unitario,
                        Cantidad = 0
                    }).ToListAsync();

                return View(model);
            }

            return RedirectToAction("Details", "Pedidos", new { id = model.Id_pedido });
        }



        // POST: DetallePedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nro_renglon,Id_pedido,Id_producto,Id_usuario,Cantidad,Descripcion,Precio_unitario,Subtotal")] DetallePedido detallePedido)
        {
            if (id != detallePedido.Id_pedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoExists(detallePedido.Id_pedido))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_pedido"] = new SelectList(_context.Pedidos, "Id_pedido", "Id_pedido", detallePedido.Id_pedido);
            ViewData["Id_producto"] = new SelectList(_context.Productos, "Id_producto", "Nombre", detallePedido.Id_producto);
            ViewData["Id_usuario"] = new SelectList(_context.Usuarios, "Id_usuario", "Apellido", detallePedido.Id_usuario);
            return View(detallePedido);
        }


       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDetalle(EditarDetallePedidoViewModel model)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id_pedido == model.Id_pedido);

            if (pedido == null || pedido.Estado != "Pendiente")
                return NotFound();

            // 1. Reintegrar stock de los detalles actuales antes de modificarlos
            foreach (var detalleExistente in pedido.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalleExistente.Id_producto);
                if (producto != null)
                {
                    producto.Stock += detalleExistente.Cantidad;
                    _context.Update(producto);
                }
            }

            // 2. Eliminar los detalles actuales
            _context.DetallePedidos.RemoveRange(pedido.Detalles);

            // 3. Crear nuevos detalles y descontar stock nuevamente
            var nuevosDetalles = new List<DetallePedido>();
            int nroRenglon = 1;
            foreach (var p in model.ProductosDisponibles.Where(p => p.Cantidad > 0))
            {
                var producto = await _context.Productos.FindAsync(p.Id_producto);
                if (producto == null)
                    continue;

                if (p.Cantidad > producto.Stock)
                {
                    ModelState.AddModelError("", $"Stock insuficiente para {producto.Nombre}.");
                    return View(model);
                }

                producto.Stock -= p.Cantidad;

                nuevosDetalles.Add(new DetallePedido
                {
                    Id_pedido = pedido.Id_pedido,
                    Nro_renglon = nroRenglon++,
                    Id_producto = p.Id_producto,
                    Cantidad = p.Cantidad,
                    Precio_unitario = producto.Precio_unitario,
                    Subtotal = producto.Precio_unitario * p.Cantidad,
                    Id_usuario = pedido.Id_usuario
                });

                _context.Update(producto);
            }

            // 4. Guardar nuevos detalles y actualizar total
            _context.DetallePedidos.AddRange(nuevosDetalles);
            pedido.Total = nuevosDetalles.Sum(d => d.Subtotal);
            _context.Update(pedido);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Pedidos", new { id = pedido.Id_pedido });
        }

        private bool DetallePedidoExists(int id)
        {
            return _context.DetallePedidos.Any(e => e.Id_pedido == id);
        }
    }
}
