using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BellaNapoli.Models;
using Microsoft.AspNetCore.Authorization;

namespace BellaNapoli.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly TestDbventa1Context _context;

        public UsuariosController(TestDbventa1Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var usuarios = await _context.Usuarios.Include(u => u.IdRolNavigation).ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(u => u.NombreCompleto != null && u.NombreCompleto.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return View(usuarios);
        }




        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            var roles = _context.Rols?.ToList(); // Previene NullReferenceException

            if (roles == null || !roles.Any())
            {
                // Opcional: podrías loguear esto o mostrar una advertencia
                roles = new List<Rol>(); // Lista vacía para evitar que falle el SelectList
            }

            ViewData["IdRol"] = new SelectList(roles, "IdRol", "Descripcion");
            ViewData["Title"] = "Crear Usuario";
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Documento,NombreCompleto,Correo,Clave,IdRol,Estado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaRegistro = DateTime.Now; // Establecer la fecha de registro actual
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var usuario = await _context.Usuarios
                                             .Include(u => u.IdRolNavigation) // Asegura que se incluya la relación
                                             .FirstOrDefaultAsync(u => u.IdUsuario == id);

                if (usuario == null)
                {
                    return NotFound();
                }

                var roles = await _context.Rols?.ToListAsync() ?? new List<Rol>();
                if (roles == null || !roles.Any())
                {
                    Console.WriteLine("No hay roles disponibles en la base de datos.");
                }

                ViewData["IdRol"] = new SelectList(roles, "IdRol", "NombreRol", usuario.IdRol);
                return View(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el usuario para editar: {ex.Message}");
                return NotFound();
            }
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Documento,NombreCompleto,Correo,Clave,IdRol,Estado")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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

            var roles = await _context.Rols?.ToListAsync() ?? new List<Rol>();
            if (roles == null || !roles.Any())
            {
                Console.WriteLine("No hay roles disponibles en la base de datos para el POST.");
            }

            ViewData["IdRol"] = new SelectList(roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
