using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using SOTA.Models.Pages.Reports;
using System;
using System.Linq;
using SOTA.Models.Pages.Reports;
using System.Security.Claims;

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
            
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);

            return View();
        }
        //
        [Route("Report/ReportsList/{tip?}")]
        public IActionResult ReportsList(int tip)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            FormirRabotaTablList Tabl = new FormirRabotaTablList();
            var rabotaList = new ReportsRabotaList();
            if (user.Role == 2)
            {
                Tabl = new FormirRabotaTablList(user.IdOo, db);
                rabotaList = Tabl.GetSpisokRabotReports();
            }
            if (user.Role == 3)
            {
                Tabl = new FormirRabotaTablList(new Mo { Id = user.IdMo }, db);
                rabotaList = Tabl.GetSpisokRabotReports();
            }
            if (user.Role == 4)
            {
                Tabl = new FormirRabotaTablList(db);
                rabotaList = Tabl.GetSpisokRabotReports();
            }
            return View("RabotaList", rabotaList);
        }

        [Route("Report/Download/{IdRabota?}/{Tip?}")]
        public IActionResult Download(int IdRabota, int Tip)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;


            ReportGenerator Report;
            if (user.Role == 2)
            {
                Report = new ReportTip1(db, IdRabota, new Oo { Id = user.IdOo });
                return RedirectToAction("File", "Download", new { path = Report.Create() });
            }

            if (user.Role == 3)
            {
                Report = new ReportTip1(db, IdRabota, new Mo { Id = user.IdMo });
                return RedirectToAction("File", "Download", new { path = Report.Create() });
            }

            if (user.Role == 4)
            {
                var mos = db.Mo.ToList();
                for (int i = 0; i < mos.Count; i++)
                {
                    try
                    {
                        Report = new ReportTip1(db, IdRabota, new Mo { Id = mos[i].Id });
                        RedirectToAction("File", "Download", new { path = Report.Create() });
                    }
                    catch
                    { }
                }
            }


            return RedirectToAction("ReportsList");
        }
        public IActionResult SaveReport(int IdRabota)
        {

            return Json("Ok");
        }
    }
}
