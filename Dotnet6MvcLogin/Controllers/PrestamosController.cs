using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Models.Domain;

namespace SistemaBiblioteca.Controllers
{
    [Authorize]
    public class PrestamosController : Controller
    {
        private readonly DatabaseContext _context;

        public PrestamosController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Prestamos

        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Prestamo == null)
            {
                return Problem("Entity set 'SistemaBibliotecContext.Libro'  is null.");
            }

            var prestamos = from m in _context.Prestamo
                            join p in _context.Libro on m.LibroId equals p.Id
                            join q in _context.Users on m.UserId equals q.Id
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                prestamos = prestamos.Where(s => s.Libro.Titulo!.Contains(searchString) || s.User.FirstName!.Contains(searchString));
            }
            return View(await prestamos.Include(x => x.Libro).Include(x => x.User).ToListAsync());
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prestamo == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public IActionResult Create()
        {
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Titulo");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Prestamos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,LibroId,TiempoPrestamo,Observaciones,FechaPrestamo")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Titulo", prestamo.LibroId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", prestamo.UserId);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prestamo == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Titulo");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,LibroId,TiempoPrestamo,Observaciones,FechaPrestamo")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Id", prestamo.LibroId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", prestamo.UserId);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prestamo == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prestamo == null)
            {
                return Problem("Entity set 'DatabaseContext.Prestamo'  is null.");
            }
            var prestamo = await _context.Prestamo.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamo.Remove(prestamo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
          return _context.Prestamo.Any(e => e.Id == id);
        }
    }
}
