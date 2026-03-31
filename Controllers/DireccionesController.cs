using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppWebDespachos.Data;
using AppWebDespachos.Models;
using System.Security.Claims;

namespace AppWebDespachos.Controllers
{
    public class DireccionesController : Controller
    {
        private readonly AppDbContext _context;

        public DireccionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Direcciones
        public async Task<IActionResult> Index()
        {
            var direcciones = _context.Direcciones
                .Include(d => d.Cliente)
                .Include(d => d.Usuario);

            return View(await direcciones.ToListAsync());
        }

        // GET: Direcciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Cliente)
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id_direccion == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Direcciones/Create
        public IActionResult Create()
        {
            ViewData["Id_cliente"] = new SelectList(_context.Clientes, "Id_cliente", "Razon_social");
          
            return View();
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_direccion,Id_cliente,Calle,Nro,Piso,Dpto,Ciudad,Cp,Provincia")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                // Obtener el usuario logueado
                var claim = User.FindFirst("Id_usuario");
                if (claim == null || !int.TryParse(claim.Value, out var idUsuario))
                {
                    return Forbid(); // o redirigir al login
                }

                direccion.Id_usuario = idUsuario;

                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Id_cliente"] = new SelectList(_context.Clientes, "Id_cliente", "Razon_social", direccion.Id_cliente);
            return View(direccion);
        }


        // GET: Direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion == null)
            {
                return NotFound();
            }
            ViewData["Id_cliente"] = new SelectList(_context.Clientes, "Id_cliente", "Razon_social", direccion.Id_cliente);
          
            return View(direccion);
        }

        // POST: Direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_direccion,Id_cliente,Id_usuario,Calle,Nro,Piso,Dpto,Ciudad,Cp,Provincia")] Direccion direccion)
        {
            if (id != direccion.Id_direccion)
            {
                return NotFound();
            }

            // Obtener el Id del usuario logueado desde los claims
            var claim = User.FindFirst("Id_usuario");

            // Asignar el Id_usuario automáticamente
            direccion.Id_usuario = int.Parse(claim.Value);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.Id_direccion))
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
            ViewData["Id_cliente"] = new SelectList(_context.Clientes, "Id_cliente", "Razon_social  ", direccion.Id_cliente);
       
            return View(direccion);
        }

        // GET: Direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Cliente)
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id_direccion == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion != null)
            {
                _context.Direcciones.Remove(direccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DireccionExists(int id)
        {
            return _context.Direcciones.Any(e => e.Id_direccion == id);
        }
    }
}
