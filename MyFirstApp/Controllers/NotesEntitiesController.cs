using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Entities;

namespace MyFirstApp.Controllers
{
    public class NotesEntitiesController : Controller
    {
        private readonly AppDbContext _context;

        public NotesEntitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NotesEntities
        public async Task<IActionResult> NotesIndex()
        {
            return _context.NotesEntities != null ?
                        View(await _context.NotesEntities.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.NotesEntities'  is null.");
        }

        // GET: NotesEntities/Details/5
        public async Task<IActionResult> NotesDetails(int? id)
        {
            if (id == null || _context.NotesEntities == null)
            {
                return NotFound();
            }

            var notesEntity = await _context.NotesEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notesEntity == null)
            {
                return NotFound();
            }

            return View(notesEntity);
        }

        // GET: NotesEntities/Create
        public IActionResult NotesCreate()
        {
            return View();
        }

        // POST: NotesEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotesCreate([Bind("Id,Name,Note")] NotesEntity notesEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notesEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(NotesIndex));
            }
            return View(notesEntity);
        }

        // GET: NotesEntities/Edit/5
        public async Task<IActionResult> NotesEdit(int? id)
        {
            if (id == null || _context.NotesEntities == null)
            {
                return NotFound();
            }

            var notesEntity = await _context.NotesEntities.FindAsync(id);
            if (notesEntity == null)
            {
                return NotFound();
            }
            return View(notesEntity);
        }

        // POST: NotesEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotesEdit(int id, [Bind("Id,Name,Note")] NotesEntity notesEntity)
        {
            if (id != notesEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notesEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotesEntityExists(notesEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(NotesIndex));
            }
            return View(notesEntity);
        }

        // GET: NotesEntities/Delete/5
        public async Task<IActionResult> NotesDelete(int? id)
        {
            if (id == null || _context.NotesEntities == null)
            {
                return NotFound();
            }

            var notesEntity = await _context.NotesEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notesEntity == null)
            {
                return NotFound();
            }

            return View(notesEntity);
        }

        // POST: NotesEntities/Delete/5
        [HttpPost, ActionName("NotesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotesEntities == null)
            {
                return Problem("Entity set 'AppDbContext.NotesEntities'  is null.");
            }
            var notesEntity = await _context.NotesEntities.FindAsync(id);
            if (notesEntity != null)
            {
                _context.NotesEntities.Remove(notesEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(NotesIndex));
        }

        private bool NotesEntityExists(int id)
        {
            return (_context.NotesEntities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
