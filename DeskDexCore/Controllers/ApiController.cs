using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskDexCore.Controllers
{
    [AllowAnonymous]
    public class ApiController : Controller
    {
        private DeskContext db;

        public ApiController(DeskContext context)
        {
            db = context;
        }

        // GET: api/Desk
        [HttpGet]
        [Route("api/floor/{floor}")]
        public async Task<FloorApiModel> GetStations(int? floor)
        {
            /* Return an overview for use on the map based on floor
             */

            try
            {
                var _floor = db.Floors.FirstOrDefaultAsync(f => f.ID == floor);
                var _stations = db.Stations.Where(s => s.Floor.ID == floor).Include(s => s.Type).Include(s => s.LastCheckin).ToListAsync();

                Floor Floor = await _floor;
                FloorApiModel am = new FloorApiModel
                {
                    Generated = DateTime.Now,
                    Stations = new List<DeskMapApiModel>(),
                    FloorImage = Floor.FilePath,
                    FloorName = Floor.Name
                };

                List<Station> Stations = await _stations;
                foreach (var Station in Stations)
                {
                    am.Stations.Add(new DeskMapApiModel
                    {
                        DeskID = Station.ID,
                        x1 = Station.x1,
                        x2 = Station.x2,
                        y1 = Station.y1,
                        y2 = Station.y2,
                        WorkStyle = Station.Type?.Name,
                        Occupied = ((Station.LastCheckin != null) && (DateTime.Now - Station.LastCheckin.LastUpdate).TotalHours < 2) ? true : false,
                        Location = Station.Location
                    });
                }

                return am;
            }
            catch
            {
                return null;
            }

        }

        [HttpGet]
        [Route("api/map/{floor}")]
        public async Task<Floor> getMap(int floor)
        {
            try
            {
                return await db.Floors.FirstOrDefaultAsync(f => f.ID == floor);
            }
            catch
            {
                return null;
            }
        }

        // GET: api/Desk/5
        [HttpGet]
        [Route("api/desk/{id}")]
        public async Task<DeskDetailApiModel> GetStation(int id)
        {
            Station station;
            try
            {
                station = await db.Stations
                    .Include(stat => stat.Type)
                    .Include(stat => stat.StationEquipments)
                    .ThenInclude(se => se.Equipment)
                    .Include(s => s.LastCheckin)
                    .FirstOrDefaultAsync(s => s.ID == id);
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }

            var ddvm = new DeskDetailApiModel
            {
                DeskID = station.ID,
                WorkStyle = station.Type.Name,
                LastUpdate = (station.LastCheckin != null) ? FormatAge(station.LastCheckin.LastUpdate) : String.Empty,
                UserName = station.LastCheckin?.Display,
                Capacity = station.Capacity,
                Equipment = new List<string>(),
                Location = station.Location,
                ImagePath = station.FilePath
            };
            foreach (var equip in db.Stations.Where(s => s.ID == id).SelectMany(e => e.StationEquipments).Select(se => se.Equipment))
            {
                ddvm.Equipment.Add(equip.Name);
            }
            return ddvm;
        }

        [HttpPost]
        [Route("api/checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckinViewModel input)
        {
            var oldCheckin = await db.Checkins.FirstOrDefaultAsync(c => c.Username == input.acid);

            if (oldCheckin != null)
            {
                var _clearCheckins = db.Stations.Where(s => s.LastCheckin == oldCheckin).ForEachAsync(s => s.LastCheckin = null);
                db.Checkins.Remove(oldCheckin);
                await _clearCheckins;
                await db.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/checkin")]
        public async Task<IActionResult> CheckIn([FromBody]CheckinViewModel input)
        {
            DateTime submitTime = DateTime.Now;

            try
            {
                var checkin = new Checkin
                {
                    LastUpdate = submitTime,
                    Username = input.acid,
                    Display = input.display
                };

                //// Update checkin table
                // check existing entries with same userID
                var oldCheckin = await db.Checkins.FirstOrDefaultAsync(c => c.Username == input.acid);

                // write checkin to database
                if (oldCheckin != null)
                {
                    // update time
                    oldCheckin.LastUpdate = submitTime;
                    oldCheckin.Display = input.display;

                    // clear out old station reg
                    await db.Stations.Where(s => s.LastCheckin.ID == oldCheckin.ID).ForEachAsync(s => s.LastCheckin = null);

                    // switch current checkin
                    checkin = oldCheckin;
                }
                else
                {
                    // make new entry
                    db.Checkins.Add(checkin);
                }

                //// Update station
                // update existing entries with same MAC
                var station = await db.Stations.FirstOrDefaultAsync(s => s.PhysicalAddress == input.address);
                if (station != null)
                {
                    // if station exists, set its checkin to this
                    station.LastCheckin = checkin;
                }
                else
                {
                    // create a new blank slate station
                    db.Stations.Add(new Station
                    {
                        PhysicalAddress = input.address,
                        Location = "Unknown",
                        StationEquipments = new List<StationEquipment>(),
                        Capacity = 1,
                        LastCheckin = checkin
                    });
                }

                // Commit changes
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        //[HttpGet("{q}")]
        [Route("api/search/{q}")]
        public async Task<SearchApiModel> Search(string q)
        {
            int searchLimit = 6;

            var _users = FindUsersAsync(q);
            var _stations = FindStationsAsync(q);

            var tmp = new SearchApiModel
            {
                People = (await _users).Take(searchLimit),
                Stations = (await _stations).Take(searchLimit)
            };
            return tmp;
        }

        private async Task<List<SearchLink>> FindStationsAsync(string term)
        {
            // look for stations

            return await db.Stations
                .Include(s => s.Floor)
                .Include(s => s.Type)
                .Where(s => s.Location.Contains(term) && s.Floor != null)
                .Select(s => new SearchLink
                {
                    Display = s.Location,
                    Link = Url.Action("Map", "Home", null) + "?floor=" + s.Floor.ID.ToString() + "&station=" + s.ID.ToString(),
                    SubText = s.Type.Name
                }).ToListAsync();
        }

        private async Task<List<SearchLink>> FindUsersAsync(string term)
        {
            // look for names on checkins
            return await db.Stations
                .Include(s => s.LastCheckin)
                .Include(s => s.Floor)
                .Where(s => s.LastCheckin.Display.Contains(term))
                .Where(s => s.Floor != null)
                .Select(s => new SearchLink
                {
                    Display = s.LastCheckin.Display,
                    Link = Url.Action("Map", "Home", null) + "?floor=" + s.Floor.ID.ToString() + "&station=" + s.ID.ToString(),
                    SubText = "Last seen " + FormatAge(s.LastCheckin.LastUpdate) + " ago"
                }).ToListAsync();
        }

        private string FormatAge(DateTime target)
        {
            TimeSpan duration = DateTime.Now - target;
            string formatted = String.Empty;

            var hours = duration.TotalHours;
            var mins = duration.TotalMinutes;

            if (mins < 60)
            {
                formatted = Math.Floor(mins).ToString() + "m";
            }
            else
            {
                if (hours < 5)
                {
                    formatted = Math.Floor(hours).ToString() + "h " + Math.Floor((mins % 60)).ToString() + "m";
                }
                else if (hours < 24)
                {
                    formatted = Math.Floor(hours).ToString() + "h";
                }
                else
                {
                    formatted = Math.Floor(duration.TotalDays).ToString() + "d";
                }
            }

            return formatted;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<bool> StationExists(int id)
        {
            return await db.Stations.CountAsync(e => e.ID == id) > 0;
        }
    }
}