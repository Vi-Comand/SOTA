﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;

namespace SOTA.Controllers
{
    [Authorize]
    public class UchenController : Controller
    {
        SotaContext db;
        string error;
        public UchenController(SotaContext context)
        {
            db = context;
        }
        // [HttpGet]
        //[Route("Rabota/RabotaAdd/")]
        [Authorize]
        public IActionResult NaznacRabotaList()
        {
            // var login = HttpContext.User.Claims.First().Value;
            var login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            FormirRabotaTablList formir = new FormirRabotaTablList(user,db);
            RabotaUchenList rabotaList = new RabotaUchenList();

            rabotaList=formir.GetSpisokRabotUchen();
            /* rabotaList.Id=RabotaList





             
             rabotaList.IdSpec = db.Specific.Where(p => p.Id == rabotaList.).ToList();
             rabotaList.SpecN = db.Specific.Where(p => p.Class == klass).ToList();
             rabotaList.PredmN = db.Predm.Where(p => p.Id == klass).ToList();
             rabotaList.Rabotas = db.Rabota.ToList();
             */
            //int KlassU = db.Klass.Where(x => x.Id == user.IdKlass).First().KlassNom;
            //RabotaTablList list = new RabotaTablList();
            //list.RabotaTabls = (from rab in db.Rabota

            //                    join SpecK in db.Specific on rab.IdSpec equals SpecK.Id into spK
            //                    from SK in spK.DefaultIfEmpty()


            //                    select new RabotaTabl
            //                    {
            //                        Id = rab.Id,
            //                        Name = rab.Name,
            //                        Dliteln = rab.Dliteln,
            //                        Nachalo = rab.Nachalo,
            //                        Konec = rab.Konec,
            //                        PredmN = db.Predm.Where(x => x.Id == SK.Predm).First().Name,
            //                        TipN = db.TipSpec.Where(x => x.Id == SK.Tip).First().Name
            //                    }).ToList();

            // list.Filt = new FilterLKTO();
        //    DateTime dateNow = DateTime.Now;
            //list.RabotaTabls = list.RabotaTabls.Where(x => x.KlassR == KlassU && x.Konec > dateNow).ToList();
            //rabotaList = list;
            //rabotaList = rabotaList.RabotaTabls

            return View(rabotaList);
        }
    }
}
