using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Controllers
{
    public class HomeController : Controller
    {


        SotaContext db;

        public HomeController(SotaContext context)
        {
            db = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            if (user.Role == 1)
            {
                ViewBag.rl = user.Role;
                return View();
            }
            if (user.Role == 0)
            {
                ViewBag.rl = user.Role;
                return RedirectToAction("NaznacRabotaList", "Uchen");
            }
            return View();
        }

        public IActionResult SpecifikacList()
        {
            return View();
        }

        public IActionResult SpecifikacAdd()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Open()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
