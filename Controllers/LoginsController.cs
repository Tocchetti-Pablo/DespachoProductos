using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using AppWebDespachos.Data;
using AppWebDespachos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AppWebDespachos.Controllers
{
    [AllowAnonymous]
    public class LoginsController : Controller
    {
        private readonly AppDbContext _context;

        public LoginsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Solo roles habilitados
            ViewData["Roles"] = new SelectList(
                _context.Roles.Where(r => r.Habilitado),
                "Id_rol", "Nombre"
            );

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string user, string password, int id_rol)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.User == user &&
                    u.Password == password &&
                    u.Id_rol == id_rol &&
                    u.Habilitado);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas o usuario inhabilitado.";
                ViewData["Roles"] = new SelectList(_context.Roles.Where(r => r.Habilitado), "Id_rol", "Nombre", id_rol);
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.User),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                new Claim("Id_usuario", usuario.Id_usuario.ToString()),
                new Claim("NombreCompleto", usuario.Nombre + " " + usuario.Apellido)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Logins");
        }

    }
}
