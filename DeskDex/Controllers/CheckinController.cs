using DeskDex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web.Http;

namespace DeskDex.Controllers
{
    public class CheckinController : ApiController
    {
        // GET: api/Checkin
        public IEnumerable<Checkin> Get()
        {
            List<Checkin> Checkins = new List<Checkin>();
            using (var db = new DeskContext())
            {
                var query = from c in db.Checkins orderby c.Username select c;
                foreach (var item in query)
                {
                    Checkins.Add((Checkin)item);
                }
            }
            return Checkins;
        }

        // GET: api/Checkin/5
        public Checkin Get(int id)
        {
            Checkin checkin = new Checkin();
            using (var db = new DeskContext())
            {
                var query = from c in db.Checkins where c.ID == id select c;
                foreach (var item in query)
                {
                    checkin = (Checkin)item;
                }
            }
            return checkin;
        }

        // PUT: api/Checkin/5
        [AcceptVerbs("POST", "PUT")]
        public IHttpActionResult Put([FromBody]string checkinMAC)
        {
            string CurrentUser = User.Identity.Name;
            DateTime submitTime = DateTime.Now;
            try
            {
                string physicalAddress = (PhysicalAddress.Parse(checkinMAC)).ToString();
                var checkin = new Checkin
                {
                    LastUpdate = submitTime,
                    Username = CurrentUser
                };

                using (var db = new DeskContext())
                {
                    //// Update checkin table
                    // check existing entries with same userID
                    var oldCheckin = db.Checkins.FirstOrDefault(c => c.Username == CurrentUser);

                    // write checkin to database
                    if (oldCheckin != null)
                    {
                        // update time
                        oldCheckin.LastUpdate = submitTime;
                    } else
                    {
                        // make new entry
                        db.Checkins.Add(checkin);
                    }

                    //// Update station
                    // update existing entries with same MAC
                    var station = db.Stations.FirstOrDefault(s => s.PhysicalAddress == physicalAddress);
                    if (station != null)
                    {
                        // if station exists, set its checkin to this
                        station.LastCheckin = checkin;
                    } else
                    {
                        // create a new blank slate station
                        db.Stations.Add(new Station
                        {
                            PhysicalAddress = physicalAddress,
                            Location = "Unknown",
                            Equipment = new List<Equipment>(),
                            Capacity = 1,
                            LastCheckin = checkin
                        });
                    }

                    // Commit changes
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        // DELETE: api/Checkin/5
        public void Delete(int id)
        {
        }
    }
}
