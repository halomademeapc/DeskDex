using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Map")]
        public IActionResult Map()
        {
            var vm = new LegendViewModel
            {
                AllWorkStyles = db.WorkStyles.ToList()
            };

            var AllFloorsList = db.Floors.ToList();
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
                ViewBag.DefaultFloor = AllFloorsList[0].ID.ToString();
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
