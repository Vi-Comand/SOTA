using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System.Diagnostics;
using System.Linq;

namespace SOTA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {


        SotaContext db;

        public HomeController(SotaContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            Users user= db.Users.Where(p => p.Name == login).First();
          

          



                ViewBag.rl = user.Role;
                if (user.Role == 4)
                {
                    ViewBag.rl = user.Role;
                    return RedirectToAction("RabotaList", "Rabota");
                }

                if (user.Role == 3)
                {
                    ViewBag.rl = user.Role;
                    return RedirectToAction("UO", "Admin");
                }

                if (user.Role == 2)
                {
                    ViewBag.rl = user.Role;
                    return RedirectToAction("OO", "Admin");
                }

                if (user.Role == 1)
                {
                    ViewBag.rl = user.Role;
                    return RedirectToAction("Klass", "Admin");
                }

                if (user.Role == 0)
                {
                    ViewBag.rl = user.Role;
                    return RedirectToAction("NaznacRabotaList", "Uchen");
                }
                else
                {
                    ViewBag.rl = user.Role;
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
