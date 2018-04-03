using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeskDex.Models;

namespace DeskDex.Controllers
{
    public class StationsController : Controller
    {
        private DeskContext db = new DeskContext();

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
            var stationViewModel = new StationViewModel
            {
                Station = new Station()

            };

            var allEquipmentList = db.Equipment.ToList();
            stationViewModel.AllEquipment = allEquipmentList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.ID.ToString()
            });

            var allTypesList = db.WorkStyles.ToList();
            stationViewModel.AllWorkStyles = allTypesList.Select(w => new SelectListItem
            {
                Text = w.Name,
                Value = w.ID.ToString()
            });

            return View(stationViewModel);
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StationViewModel stationViewModel)
        {
            if (ModelState.IsValid)
            {
                var station = stationViewModel.Station;
               
                // update model
                station.Equipment = db.Equipment.Where(o => stationViewModel.SelectedEquipment.Contains(o.ID)).ToList();
                station.Type = db.WorkStyles.Find(stationViewModel.selectedWorkStyle);

                db.Stations.Add(station);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stationViewModel);
        }

        // GET: Stations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stationViewModel = new StationViewModel
            {
                Station = db.Stations.Find(id)

            };
            if (stationViewModel.Station == null)
            {
                return HttpNotFound();
            }

            var allEquipmentList = db.Equipment.ToList();
            stationViewModel.AllEquipment = allEquipmentList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.ID.ToString()
            });

            var allTypesList = db.WorkStyles.ToList();
            stationViewModel.AllWorkStyles = allTypesList.Select(w => new SelectListItem
            {
                Text = w.Name,
                Value = w.ID.ToString()
            });

            return View(stationViewModel);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StationViewModel stationViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("DELETE FROM StationEquipments where Station_ID = @ID", new SqlParameter("@ID",stationViewModel.Station.ID) );

                var station = stationViewModel.Station;

                var oldEntry = db.Stations.Find(station.ID);

                // update old row
                db.Entry(oldEntry).CurrentValues.SetValues(station);
                oldEntry.Equipment = db.Equipment.Where(o => stationViewModel.SelectedEquipment.Contains(o.ID)).ToList();
                oldEntry.Type = db.WorkStyles.Find(stationViewModel.selectedWorkStyle);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stationViewModel);
        }

        // GET: Stations/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Station station = db.Stations.Find(id);
            db.Stations.Remove(station);
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
