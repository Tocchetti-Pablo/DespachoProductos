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
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Clientes.Include(c => c.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id_cliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["Id_usuario"] = new SelectList(
                _context.Usuarios
                    .Where(u => u.Habilitado) // opcional: solo usuarios habilitados
                    .Select(u => new {
                        u.Id_usuario,
                        NombreCompleto = u.Nombre + " " + u.Apellido
                    }),
                "Id_usuario",
                "NombreCompleto"
            );
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_cliente,Razon_social,Descripcion,Cuit,Email,Telefono")] Cliente cliente)
        {
            // Obtener el Id del usuario logueado desde los claims
            var claim = User.FindFirst("Id_usuario");

            if (claim == null)
            {
                // Si no está logueado o falta el claim, redirigimos al login
                return RedirectToAction("Index", "Logins");
            }

            // Asignar el Id_usuario automáticamente
            cliente.Id_usuario = int.Parse(claim.Value);

            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Ya no necesitamos ViewData["Id_usuario"] porque el usuario no lo selecciona
            return View(cliente);
        }


        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            // Ya no necesitas ViewData ni select de usuarios porque se asigna automáticamente en POST
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_cliente,Razon_social,Descripcion,Cuit,Email,Telefono")] Cliente cliente)
        {
            if (id != cliente.Id_cliente)
            {
                return NotFound();
            }

            var claim = User.FindFirst("Id_usuario");
            if (claim == null)
            {
                return RedirectToAction("Index", "Logins");
            }

            // Reasignamos el usuario actual
            cliente.Id_usuario = int.Parse(claim.Value);

            if (ModelState.IsValid)
            {
                try
                {
                    // Traer cliente original de la DB con el estado actual
                    var clienteOriginal = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id_cliente == id);
                    if (clienteOriginal == null)
                        return NotFound();

                    // Mantener el estado Habilitado original sin modificarlo
                    cliente.Habilitado = clienteOriginal.Habilitado;

                    // Actualizar solo los campos editables
                    _context.Entry(cliente).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id_cliente))
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

            return View(cliente);
        }


        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id_cliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clienteExistente = await _context.Clientes.FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound();
            }

            // Invierte el estado actual
            clienteExistente.Habilitado = !clienteExistente.Habilitado;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id_cliente == id);
        }
    }
}
