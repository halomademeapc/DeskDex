using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskDexCore.Controllers
{
    [AllowAnonymous]
    public class ApiController : Controller
    {
        private DeskContext db;
        private static readonly string GRAPH_URI = "https://graph.windows.net/halomademeapc.com/";

        public ApiController(DeskContext context)
        {
            db = context;
        }

        // GET: api/Desk
        [HttpGet]
        [Route("api/floor/{floor}")]
        public FloorApiModel GetStations(int? floor)
        {
            /* Return an overview for use on the map based on floor
             */

            try
            {
                Floor Floor = db.Floors.Find(floor);

                FloorApiModel am = new FloorApiModel
                {
                    Generated = DateTime.Now,
                    Stations = new List<DeskMapApiModel>(),
                    FloorImage = Floor.FilePath,
                    FloorName = Floor.Name
                };

                List<Station> Stations = db.Stations.Where(s => s.Floor.ID == floor).Include(s => s.Type).Include(s => s.LastCheckin).ToList();

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
                        LastCheckin = Station.LastCheckin?.LastUpdate,
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
        public Floor getMap(int floor)
        {
            try
            {
                return db.Floors.Find(floor);
            }
            catch
            {
                return null;
            }
        }

        // GET: api/Desk/5
        [HttpGet]
        [Route("api/desk/{id}")]
        public DeskDetailApiModel GetStation(int id)
        {
            Station station;
            try
            {
                station = db.Stations.Where(s => s.ID == id).Include(stat => stat.Type).Include(stat => stat.StationEquipments).ThenInclude(se => se.Equipment).Include(s => s.LastCheckin).First();
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }

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
            foreach (var equip in db.Stations.Where(s => s.ID == id).SelectMany(e => e.StationEquipments).Select(se => se.Equipment))
            {
                ddvm.Equipment.Add(equip.Name);
            }
            return ddvm;
        }

        [Route("api/checkin")]
        [HttpPost]
        public ActionResult Post([FromBody]CheckinViewModel input)
        {
            DateTime submitTime = DateTime.Now;

            try
            {
                var checkin = new Checkin
                {
                    LastUpdate = submitTime,
                    Username = input.acid
                };


                //// Update checkin table
                // check existing entries with same userID
                var oldCheckin = db.Checkins.FirstOrDefault(c => c.Username == input.acid);

                // write checkin to database
                if (oldCheckin != null)
                {
                    // update time
                    oldCheckin.LastUpdate = submitTime;

                    // clear out old station reg
                    var prevReg = from s in db.Stations where s.LastCheckin.ID == oldCheckin.ID select s;
                    foreach (var item in prevReg)
                    {
                        item.LastCheckin = null;
                    }

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
                var station = db.Stations.FirstOrDefault(s => s.PhysicalAddress == input.address);
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
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        [Route("api/search")]
        [HttpGet("{term}")]
        public async Task<IActionResult> Search(string term)
        {
            int searchLimit = 6;
            return Ok(new SearchApiModel
            {
                People = (await FindUsers(term)).Take(searchLimit),
                Stations = (await FindStations(term)).Take(searchLimit)
            });
        }

        private async Task<List<SearchLink>> FindStations(string term)
        {
            // look for stations
            return await (db.Stations.Where(s => s.Location.Contains(term)).Select(s => new SearchLink { Display = s.Location, Link = s.ID.ToString() }).ToListAsync());
        }

        private async Task<List<SearchLink>> FindUsers(string term)
        {
            // look for users in aad
            HttpClient client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var result = new List<SearchLink>();

            queryString["api-version"] = "1.6";
            queryString["$filter"] = "startswith(displayName,'" + term + "'";

            var uri = GRAPH_URI + "users?" + queryString;
            try
            {
                var response = await client.GetAsync(uri);

                if (response.Content != null)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);

                    foreach (var user in responseJson.value)
                    {
                        result.Add(new SearchLink
                        {
                            Display = (string)user.displayName ?? (user.givenName + " " + user.surname) ?? String.Empty,
                            Link = (string)user.objectId ?? user.userPrincipalName ?? user.mailNickname ?? String.Empty
                        });
                    }
                }
            }
            catch
            {
                result.Add(new SearchLink { Display = "Unable to connect to Azure AD", Link = String.Empty });
            }

            return result;
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