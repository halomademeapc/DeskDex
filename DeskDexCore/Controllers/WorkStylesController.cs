using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeskDexCore.Controllers
{
    public class WorkStylesController : Controller
    {
        private DeskContext db;

        public WorkStylesController(DeskContext context)
        {
            db = context;
        }

        // GET: WorkStyles
        public ActionResult Index()
        {
            return View(db.WorkStyles.ToList());
        }

        // GET: WorkStyles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return StatusCode(500);
            }
            WorkStyle workStyle = db.WorkStyles.Find(id);
            if (workStyle == null)
            {
                return StatusCode(404);
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
        public ActionResult Create(WorkStyle workStyle)
        {
            if (ModelState.IsValid)
            {
                db.WorkStyles.Add(workStyle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workStyle);
        }

        // GET: WorkStyles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode(500);
            }
            WorkStyle workStyle = db.WorkStyles.Find(id);
            if (workStyle == null)
            {
                return StatusCode(404);
            }
            return View(workStyle);
        }

        // POST: WorkStyles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkStyle workStyle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workStyle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workStyle);
        }

        // GET: WorkStyles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(500);
            }
            WorkStyle workStyle = db.WorkStyles.Find(id);
            if (workStyle == null)
            {
                return StatusCode(404);
            }
            return View(workStyle);
        }

        // POST: WorkStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkStyle workStyle = db.WorkStyles.Find(id);
            db.WorkStyles.Remove(workStyle);
            db.SaveChanges();
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
