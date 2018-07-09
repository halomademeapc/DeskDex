using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeskDexCore.Controllers
{
    [AllowAnonymous]
    [Route("")]
    public class HomeController : Controller
    {
        private DeskContext db;

        public HomeController(DeskContext context)
        {
            db = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var stats = new HomeViewModel
            {
                UserCount = await db.Checkins.Where(c => (DateTime.Now - c.LastUpdate).TotalHours < 2).CountAsync(),
                StationCount = await db.Stations.Where(s => s.Location != "Unknown").CountAsync()
            };
            return View(stats);
        }

        [HttpGet("Map")]
        public async Task<IActionResult> Map()
        {
            // start db calls
            var workStyles = db.WorkStyles.ToListAsync();
            var allFloors = db.Floors.ToListAsync();

            // assign results
            var vm = new LegendViewModel
            {
                AllWorkStyles = await workStyles
            };

            var AllFloorsList = await allFloors;
            vm.AllFloors = AllFloorsList.OrderBy(f => f.SortName).Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.ID.ToString()
            });

            if (Request.Cookies["mapFloor"] != null)
            {
                ViewBag.DefaultFloor = Request.Cookies["mapFloor"].ToString();
            }
            else
            {
                ViewBag.DefaultFloor = AllFloorsList.FirstOrDefault().ID.ToString();
            }

            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
