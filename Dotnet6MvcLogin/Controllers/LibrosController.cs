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
    public class LibrosController : Controller
    {
        private readonly DatabaseContext _context;

        public LibrosController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Libro == null)
            {
                return Problem("Entity set 'SistemaBibliotecContext.Libro'  is null.");
            }

            var libros = from m in _context.Libro
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(s => s.Titulo!.Contains(searchString) || s.Autor!.Contains(searchString));
            }
            return View(await libros.ToListAsync());
        }
        public IActionResult Consulta()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConsultarLibro(string searchString)
        {
            return View();
        }
        public class RequestToSearch
        {
            public string Autor { get; set; }
            public string Titulo { get; set; }
            public string Estado { get; set; }
            public string TipoLibro { get; set; }
        }
        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libro == null)
            {
                return NotFound();
            }

            var libro = await _context.Libro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Autor,Titulo,Materia,Estado,TipoLibro,CategoriaEdad")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                libro.UpdatedAt = DateTime.UtcNow.AddHours(-5);
                libro.CreatedAt = DateTime.UtcNow.AddHours(-5);
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libro == null)
            {
                return NotFound();
            }

            var libro = await _context.Libro.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Autor,Titulo,Materia,Estado,TipoLibro,CategoriaEdad,CreatedAt,UpdatedAt")] Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
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
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libro == null)
            {
                return NotFound();
            }

            var libro = await _context.Libro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libro == null)
            {
                return Problem("Entity set 'SistemaBibliotecContext.Libro'  is null.");
            }
            var libro = await _context.Libro.FindAsync(id);
            if (libro != null)
            {
                _context.Libro.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
          return _context.Libro.Any(e => e.Id == id);
        }
    }
}
