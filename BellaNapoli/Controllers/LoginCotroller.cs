using BellaNapoli.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BellaNapoli.Controllers
{
    public class LoginController : Controller
    {
        private readonly TestDbventa1Context _context;

        public LoginController(TestDbventa1Context context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string correo, string clave)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
            {
                ModelState.AddModelError(string.Empty, "Correo y clave son requeridos.");
                return View();
            }

            var usuario = await _context.Usuarios
                                        .Include(u => u.IdRolNavigation)
                                        .FirstOrDefaultAsync(u => u.Correo == correo && u.Clave == clave);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View();
            }

            var rol = await _context.Rols.Where(x => x.IdRol == (int)usuario.IdRol).FirstOrDefaultAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? (usuario.Correo ?? "")),
                new Claim(ClaimTypes.Role, rol.Descripcion ?? "EMPLEADO")
            };

            var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ClaimTypes.Role
        );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });

            return RedirectToAction("Index", "Home");
        }
    }
}
