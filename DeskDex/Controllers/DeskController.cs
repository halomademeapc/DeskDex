using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DeskDex.Models;

namespace DeskDex.Controllers
{
    [AllowAnonymous]
    public class DeskController : ApiController
    {
        private DeskContext db = new DeskContext();

        // GET: api/Desk
        [Route("api/map/{floor}")]
        public IEnumerable<DeskMapViewModel> GetStations(int? floor)
        {
            /* Return an overview for use on the map based on floor
             */
            List<DeskMapViewModel> vm = new List<DeskMapViewModel>();
            //var query;
            List<Station> Stations;
            
            if(floor != null)
            {
                Stations = db.Stations.Where(s => s.Floor == floor).ToList();
            } else
            {
                Stations = db.Stations.ToList();
            }

            foreach (var Station in Stations)
            {
                vm.Add(new DeskMapViewModel
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
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Desk/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStation(int id, Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.ID)
            {
                return BadRequest();
            }

            db.Entry(station).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Desk
        [ResponseType(typeof(Station))]
        public IHttpActionResult PostStation(Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stations.Add(station);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = station.ID }, station);
        }

        // DELETE: api/Desk/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult DeleteStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            db.Stations.Remove(station);
            db.SaveChanges();

            return Ok(station);
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