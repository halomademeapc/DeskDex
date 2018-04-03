using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DeskDex.Models;
using WebMatrix.WebData;

namespace DeskDex.Controllers
{
    public class StationsController : Controller
    {
        private static readonly string AD_GROUP = @"IFB\WebAdmin";
        private DeskContext db = new DeskContext();

        private static bool IsAdmin()
        {
            System.Security.Principal.WindowsIdentity MyIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal MyPrincipal = new System.Security.Principal.WindowsPrincipal(MyIdentity);
            return MyPrincipal.IsInRole(AD_GROUP);
        }

        // GET: Stations
        public ActionResult Index()
        {
            return View(db.Stations.ToList());
        }

        // GET: Stations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // GET: Stations/Create
        public ActionResult Create()
        {
            if (IsAdmin())
            {
                // Allow access
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PhysicalAddress,Location,Capacity,x1,y1,x2,y2")] Station station)
        {
            if (IsAdmin())
            {
                // Allow access
                if (ModelState.IsValid)
                {
                    db.Stations.Add(station);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(station);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Stations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsAdmin())
            {
                // Allow access
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Station station = db.Stations.Find(id);
                if (station == null)
                {
                    return HttpNotFound();
                }
                return View(station);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PhysicalAddress,Location,Capacity,x1,y1,x2,y2")] Station station)
        {
            if (IsAdmin())
            {
                // Allow access
                if (ModelState.IsValid)
                {
                    db.Entry(station).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(station);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Stations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (IsAdmin())
            {
                // Allow access
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Station station = db.Stations.Find(id);
                if (station == null)
                {
                    return HttpNotFound();
                }
                return View(station);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (IsAdmin())
            {
                // Allow access
                Station station = db.Stations.Find(id);
                db.Stations.Remove(station);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
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
