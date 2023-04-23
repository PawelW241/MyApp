using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Entities;
using MyFirstApp.Models;

namespace MyFirstApp.Controllers
{
    public class CalendarEntitiesController : Controller
    {
        private readonly AppDbContext _context;

        public CalendarEntitiesController(AppDbContext context)
        {
            _context = context;
        }

        private void DateChange(int id)
        {
            switch(id)
            {
                case 0:
                    CalendarModel.Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    break;
                case 1:
                    if (CalendarModel.Today.Month == 12) { CalendarModel.Today = new DateTime((int)CalendarModel.Today.Year + 1, 1, CalendarModel.Today.Day); break; }
                    CalendarModel.Today = new DateTime(CalendarModel.Today.Year, (int)CalendarModel.Today.Month + 1, CalendarModel.Today.Day);
                    break;
                case -1:
                    if (CalendarModel.Today.Month == 1) { CalendarModel.Today = new DateTime((int)CalendarModel.Today.Year - 1, 12, CalendarModel.Today.Day); break; }
                    CalendarModel.Today = new DateTime(CalendarModel.Today.Year, (int)CalendarModel.Today.Month - 1, CalendarModel.Today.Day);
                    break;
                case 2:
                    CalendarModel.Today = new DateTime(CalendarModel.Today.Year + 1, (int)CalendarModel.Today.Month, CalendarModel.Today.Day);
                    break;
                case -2:
                    CalendarModel.Today = new DateTime(CalendarModel.Today.Year - 1, (int)CalendarModel.Today.Month, CalendarModel.Today.Day);
                    break;
            }
                
        }

        // GET: CalendarEntities
        public async Task<IActionResult> CalendarIndex(int Id)
        {
            var IndexContext = _context.CalendarEntities.Where(x => x.Date.Day == Id && x.Date.Month == CalendarModel.Today.Month && x.Date.Year == CalendarModel.Today.Year).OrderBy(n => n.Date);
            CalendarModel.Today = new DateTime(CalendarModel.Today.Year, CalendarModel.Today.Month, (int)Id);
            return IndexContext != null ?
                           View(await IndexContext.ToListAsync()) :
                           Problem("Entity set 'AppDbContext.CalendarEntities'  is null.");
        }

        // GET: CalendarEntities/Details/5
        public async Task<IActionResult> CalendarDetails(int? id)
        {
            if (id == null || _context.CalendarEntities == null)
            {
                return NotFound();
            }
            var calendarEntity = await _context.CalendarEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calendarEntity == null)
            {
                return NotFound();
            }
            return View(calendarEntity);
        }

        public async Task<IActionResult> CalendarView(int id)
        {
            DateChange(id);
            var calendarEntity = await _context.CalendarEntities.ToListAsync();
            CalendarModel.Set();
            return View(calendarEntity);
        }

        // GET: CalendarEntities/Create
        public IActionResult CalendarCreate()
        {
            return View();
        }

        // POST: CalendarEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalendarCreate([Bind("Id,Date,DateEnd,Title,Description")] CalendarEntity calendarEntity)
        {
            if (ModelState.IsValid)
            {
                if(calendarEntity.Date > calendarEntity.DateEnd) 
                    { 
                    ModelState.AddModelError("DateEnd","DateEnd field failure");
                    return View();
                    }

                _context.Add(calendarEntity);
                await _context.SaveChangesAsync();
                CalendarModel.Today = new DateTime(calendarEntity.Date.Year, calendarEntity.Date.Month, calendarEntity.Date.Day);
                return RedirectToAction(nameof(CalendarIndex), new { id = CalendarModel.Today.Day });
            }
            return View(calendarEntity);
        }

        // GET: CalendarEntities/Edit/5
        public async Task<IActionResult> CalendarEdit(int? id)
        {
            if (id == null || _context.CalendarEntities == null)
            {
                return NotFound();
            }
            var calendarEntity = await _context.CalendarEntities.FindAsync(id);
            if (calendarEntity == null)
            {
                return NotFound();
            }
            return View(calendarEntity);
        }

        // POST: CalendarEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalendarEdit(int id, [Bind("Id,Date,DateEnd,Title,Description")] CalendarEntity calendarEntity)
        {
            if (id != calendarEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (calendarEntity.Date > calendarEntity.DateEnd)
                {
                    ModelState.AddModelError("DateEnd", "DateEnd field failure");
                    return View();
                }

                try
                {
                    _context.Update(calendarEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarEntityExists(calendarEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CalendarModel.Today = new DateTime(calendarEntity.Date.Year, calendarEntity.Date.Month, calendarEntity.Date.Day);
                return RedirectToAction(nameof(CalendarIndex), new { id = CalendarModel.Today.Day });
            }
            return View(calendarEntity);
        }

        // GET: CalendarEntities/Delete/5
        public async Task<IActionResult> CalendarDelete(int? id)
        {
            if (id == null || _context.CalendarEntities == null)
            {
                return NotFound();
            }
            var calendarEntity = await _context.CalendarEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calendarEntity == null)
            {
                return NotFound();
            }
            return View(calendarEntity);
        }

        // POST: CalendarEntities/Delete/5
        [HttpPost, ActionName("CalendarDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CalendarEntities == null)
            {
                return Problem("Entity set 'AppDbContext.CalendarEntities'  is null.");
            }
            var calendarEntity = await _context.CalendarEntities.FindAsync(id);
            if (calendarEntity != null)
            {
                _context.CalendarEntities.Remove(calendarEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CalendarIndex), new { id = CalendarModel.Today.Day });
        }

        private bool CalendarEntityExists(int id)
        {
            return (_context.CalendarEntities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
