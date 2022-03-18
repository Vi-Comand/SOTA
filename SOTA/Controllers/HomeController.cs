using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace SOTA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {


        SotaContext db;

        public HomeController(SotaContext context)
        { var rr=7;
            db = context;
        }
        public IActionResult Index()
        {

            int role;
            try
            {
                role = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            }
            catch
            {
                string login = HttpContext.User.Identity.Name;
                role = db.Users.Where(p => p.Name == login).First().Role;
            }









            ViewBag.rl = role;
            if (role == 4)
            {

                return RedirectToAction("RabotaList", "Rabota");
            }

            if (role == 3)
            {

                return RedirectToAction("UO", "Admin");
            }

            if (role == 2)
            {

                return RedirectToAction("OO", "Admin");
            }

            if (role == 1)
            {

                return RedirectToAction("Klass", "Admin");
            }

            if (role == 0)
            {

                return RedirectToAction("NaznacRabotaList", "Uchen");
            }
            else
            {

                return RedirectToAction("RabotaList", "Rabota");
            }

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
