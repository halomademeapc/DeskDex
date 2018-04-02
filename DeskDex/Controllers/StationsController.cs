using DeskDex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeskDex.Controllers
{
    public class StationsController : ApiController
    {
        public static List<Equipment> EquipmentTypes = new List<Equipment>
        {
            new Equipment("Docking Station", "Dell WD-15 USB Type-C Dock for Laptops"),
            new Equipment("WYSE Terminal", "Thin client for access to the virtual environment"),
            new Equipment("Phone", "Cisco IP phone system"),
            new Equipment("Standing Desk","Desk with adjustable height"),
            new Equipment("Standing Desk (Extra Tall)", "Desk with adjustable height up to XX inches"),
            new Equipment("USB Power Outlet", "2-outlet access point with USB connectors for charging devices"),
            new Equipment("Whiteboard", "Erasable surface for notetaking and collaboration"),
            new Equipment("MirrorOp Display", "Large display enabled for wireless sharing from a laptop"),
            new Equipment("SmartBoard", "Interactive display for collaboration")
        };

        public static List<WorkStyle> WorkStyles = new List<WorkStyle> {
            new WorkStyle("Agile"),
            new WorkStyle("Unassigned Resident"),
            new WorkStyle("Assigned Resident")
        };

        public static List<Checkin> Checkins = new List<Checkin>
        {
            new Checkin
            {
                Username = "IFB\agriffith",
                LastUpdate = DateTime.Now
            }
        };

        public static List<Station> Stations = new List<Station>
        {
            new Station
            {
                PhysicalAddress = (System.Net.NetworkInformation.PhysicalAddress.Parse("28-16-AD-58-D8-33")).ToString(),
                    Location = "N-4-D621",
                    Equipment = new List<Equipment>
                    {
                        EquipmentTypes[0],
                        EquipmentTypes[2],
                        EquipmentTypes[3]
                    },
                    Type = WorkStyles[0],
                    LastCheckin = Checkins[0],
                    Capacity = 1
            }
        };

        public IEnumerable<Station> GetStations()
        {
            return Stations;
        }

        public IHttpActionResult GetStation(int ID)
        {
            var Station = Stations.FirstOrDefault((s) => s.ID == ID);
            if (Station == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Station);
            }
        }
    }
}
