using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeskDexCore.Models;

namespace DeskDexCore.Controllers
{
    [Authorize]
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
            if (Request.Cookies["mapFloor"] != null)
            {
                ViewBag.DefaultFloor = Request.Cookies["mapFloor"].ToString();
            }
            return View(db.WorkStyles.ToList());
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
