using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskDexCore.Controllers
{
    [AllowAnonymous]
    public class DeskController : Controller
    {
        private DeskContext db = new DeskContext();

        // GET: api/Desk
        [Route("api/map/{floor}")]
        public IEnumerable<DeskMapApiModel> GetStations(int? floor)
        {
            /* Return an overview for use on the map based on floor
             */
            List<DeskMapApiModel> vm = new List<DeskMapApiModel>();
            //var query;
            List<Station> Stations;

            if (floor != null)
            {
                Stations = db.Stations.Where(s => s.Floor == floor).ToList();
            }
            else
            {
                Stations = db.Stations.ToList();
            }

            foreach (var Station in Stations)
            {
                vm.Add(new DeskMapApiModel
                {
                    DeskID = Station.ID,
                    x1 = Station.x1,
                    x2 = Station.x2,
                    y1 = Station.y1,
                    y2 = Station.y2,
                    WorkStyle = Station.Type?.Name,
                    LastCheckin = Station.LastCheckin?.LastUpdate,
                    Location = Station.Location
                });
            }

            return vm;
        }

        // GET: api/Desk/5
        public DeskDetailApiModel GetStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return null;
            }
            else
            {
                var ddvm = new DeskDetailApiModel
                {
                    DeskID = station.ID,
                    WorkStyle = station.Type.Name,
                    LastUpdate = station.LastCheckin?.LastUpdate,
                    UserName = station.LastCheckin?.Username,
                    Capacity = station.Capacity,
                    Equipment = new List<string>(),
                    Location = station.Location,
                    ImagePath = station.FilePath
                };
                foreach (var equip in station.Equipment)
                {
                    ddvm.Equipment.Add(equip.Name);
                }
                return ddvm;
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

        private bool StationExists(int id)
        {
            return db.Stations.Count(e => e.ID == id) > 0;
        }
    }
}