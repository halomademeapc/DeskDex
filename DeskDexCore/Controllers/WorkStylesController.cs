using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeskDexCore.Models;

namespace DeskDexCore.Controllers
{
    public class WorkStylesController : Controller
    {
        private readonly DeskContext _context;

        public WorkStylesController(DeskContext context)
        {
            _context = context;
        }

        // GET: WorkStyles
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkStyles.ToListAsync());
        }

        // GET: WorkStyles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workStyle = await _context.WorkStyles
                .SingleOrDefaultAsync(m => m.ID == id);
            if (workStyle == null)
            {
                return NotFound();
            }

            return View(workStyle);
        }

        // GET: WorkStyles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkStyles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Argb")] WorkStyle workStyle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workStyle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workStyle);
        }

        // GET: WorkStyles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workStyle = await _context.WorkStyles.SingleOrDefaultAsync(m => m.ID == id);
            if (workStyle == null)
            {
                return NotFound();
            }
            return View(workStyle);
        }

        // POST: WorkStyles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Argb")] WorkStyle workStyle)
        {
            if (id != workStyle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workStyle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkStyleExists(workStyle.ID))
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
            return View(workStyle);
        }

        // GET: WorkStyles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workStyle = await _context.WorkStyles
                .SingleOrDefaultAsync(m => m.ID == id);
            if (workStyle == null)
            {
                return NotFound();
            }

            return View(workStyle);
        }

        // POST: WorkStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workStyle = await _context.WorkStyles.SingleOrDefaultAsync(m => m.ID == id);
            _context.WorkStyles.Remove(workStyle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkStyleExists(int id)
        {
            return _context.WorkStyles.Any(e => e.ID == id);
        }
    }
}
