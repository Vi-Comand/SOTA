using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;

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
        //[Route("Rabota/RabotaAdd/")]
        [Authorize]
        public IActionResult ReportsList()
        {
            // var login = HttpContext.User.Claims.First().Value;
            var login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            RabotaTablList rabotaList = new RabotaTablList();
            int KlassU = db.Klass.Where(x => x.Id == user.IdKlass).First().KlassNom;
            RabotaTablList list = new RabotaTablList();
            list.RabotaTabls = (from rab in db.Rabota

                                join SpecK in db.Specific on rab.IdSpec equals SpecK.Id into spK
                                from SK in spK.DefaultIfEmpty()


                                select new RabotaTabl
                                {
                                    Id = rab.Id,
                                    Name = rab.Name,
                                    IdSpec = rab.IdSpec,
                                    Dliteln = rab.Dliteln,
                                    UrovenRabot = rab.UrovenRabot,
                                    Nachalo = rab.Nachalo,
                                    Konec = rab.Konec,
                                    ListUchasn = rab.ListUchasn,
                                    Sozd = rab.Sozd,
                                    SpecN = SK.Name,
                                    PredmN = db.Predm.Where(x => x.Id == SK.Predm).First().Name,
                                    TipN = db.TipSpec.Where(x => x.Id == SK.Tip).First().Name,
                                    KlassR = SK.Class
                                }).ToList();

            // list.Filt = new FilterLKTO();
            DateTime dateNow = DateTime.Now;
            list.RabotaTabls = list.RabotaTabls.Where(x => x.KlassR == KlassU && x.Konec < dateNow).ToList();
            rabotaList = list;
            //rabotaList = rabotaList.RabotaTabls

            return View(rabotaList);
        }
    }
}
