using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeskDex.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Map()
        {
            if (Request.Cookies["mapFloor"] != null)
            {
                ViewBag.DefaultFloor = Request.Cookies["mapFloor"].Value.ToString();
            }
            return View();
        }
    }
}