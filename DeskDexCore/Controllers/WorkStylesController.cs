using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeskDexCore.Controllers
{
    [Authorize(Policy = "Admins")]
    public class WorkStylesController : Controller
    {
        private DeskContext db;

        public WorkStylesController(DeskContext context)
        {
            db = context;
        }

        // GET: WorkStyles
        public async Task<IActionResult> Index()
        {
            return View(await db.WorkStyles.ToListAsync());
        }

        // GET: WorkStyles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            WorkStyle workStyle = await db.WorkStyles.FirstOrDefaultAsync(w => w.ID == id);
            if (workStyle == null)
            {
                return NotFound();
            }
            return View(workStyle);
        }

        // GET: WorkStyles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkStyles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkStyle workStyle)
        {
            if (ModelState.IsValid)
            {
                db.WorkStyles.Add(workStyle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
            WorkStyle workStyle = await db.WorkStyles.FirstOrDefaultAsync(w => w.ID == id);
            if (workStyle == null)
            {
                return NotFound();
            }
            return View(workStyle);
        }

        // POST: WorkStyles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkStyle workStyle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workStyle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
            WorkStyle workStyle = await db.WorkStyles.FirstOrDefaultAsync(w => w.ID == id);
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
            WorkStyle workStyle = await db.WorkStyles.FirstOrDefaultAsync(w => w.ID == id);
            db.WorkStyles.Remove(workStyle);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
