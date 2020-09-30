using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;
using SOTA.Models.Pages.Reports;


namespace SOTA.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        SotaContext db;
        string error;
        public ReportController(SotaContext context)
        {
            db = context;
        }
        // [HttpGet]
       
        [Authorize]
        [Route("Report/ReportsList/")]
        public IActionResult ReportsList()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;

            return View();
        }
        [Route("Report/ReportsList/{tip?}")]
        public IActionResult ReportsList(int tip)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            int idOO = 16;//iro
            FormirRabotaTablList Tabl = new FormirRabotaTablList(idOO, db);
            var rabotaList = Tabl.GetSpisokRabotReports();
            return View("RabotaList",rabotaList);
        }

        [Route("Report/Download/{IdRabota?}/{Tip?}")]
        public IActionResult Download(int IdRabota,int Tip)
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            //ViewBag.rl = user.Role;
            int idOO = 16;//iro

            

                     ReportGenerator Report = new ReportGenerator(Tip, db, IdRabota);
            Report.Generaition();




           return RedirectToAction("File","Download");
        }
        public IActionResult SaveReport(int IdRabota)
        {

            return Json("Ok");
        }
    }
}
