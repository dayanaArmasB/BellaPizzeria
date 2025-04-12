using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BellaNapoli.Models;

namespace BellaNapoli.Controllers
{
    public class NegociosController : Controller
    {
        private readonly TestDbventa1Context _context;

        public NegociosController(TestDbventa1Context context)
        {
            _context = context;
        }

        // GET: Negocios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Negocios.ToListAsync());
        }

        // GET: Negocios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _context.Negocios
                .FirstOrDefaultAsync(m => m.IdNegocio == id);
            if (negocio == null)
            {
                return NotFound();
            }

            return View(negocio);
        }

        // GET: Negocios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Negocios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNegocio,Nombre,Ruc,Direccion,Logo")] Negocio negocio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(negocio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(negocio);
        }

        // GET: Negocios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _context.Negocios.FindAsync(id);
            if (negocio == null)
            {
                return NotFound();
            }
            return View(negocio);
        }

        // POST: Negocios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNegocio,Nombre,Ruc,Direccion,Logo")] Negocio negocio)
        {
            if (id != negocio.IdNegocio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(negocio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NegocioExists(negocio.IdNegocio))
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
            return View(negocio);
        }

        // GET: Negocios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _context.Negocios
                .FirstOrDefaultAsync(m => m.IdNegocio == id);
            if (negocio == null)
            {
                return NotFound();
            }

            return View(negocio);
        }

        // POST: Negocios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var negocio = await _context.Negocios.FindAsync(id);
            if (negocio != null)
            {
                _context.Negocios.Remove(negocio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NegocioExists(int id)
        {
            return _context.Negocios.Any(e => e.IdNegocio == id);
        }
    }
}
