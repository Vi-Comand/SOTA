using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using SOTA.Models.Pages.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//TODO: Скрывать прошедшие работы поле в базе hiden, в таблиу добавить chekbox 

namespace SOTA.Controllers
{
    [Authorize]
    public class RabotaController : Controller
    {
        SotaContext db;
        string error;
        public RabotaController(SotaContext context)
        {
            db = context;
        }
        // [HttpGet]
        //[Route("Rabota/RabotaAdd/")]
        public IActionResult RabotaAdd()
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            ViewData["Predms"] = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            ViewData["SpisSpec"] = db.Specific;
            return View();
        }

        //[Route("Rabota/RabotaAdd/{IdRabota?}")]
        //  [HttpPost]

        public IActionResult CleanAllStaticSpec()
        {
            StaticRabotsOtvVBD.CleanStaticOtvVBDs();
            return RedirectToAction("RabotaList");
        }


        public IActionResult RabotaSozdan()
        {
            Rabota rabota = new Rabota();
            db.Rabota.Add(rabota);
            db.SaveChanges();
            int IdRabota = rabota.Id;
            return RedirectToAction("RabotaRedact", new { IdRabota });
        }

        public IActionResult RabotaRedact(int IdRabota)
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            SborkaRabotaRedact sbor = new SborkaRabotaRedact(db, IdRabota);
            RabotaRedact rabota = sbor.GetRabotaRedact();






            // ViewData["TipSpecs"] = db.TipSpec.ToList();


            return View(rabota);

        }
        public IActionResult RabotaList()
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            RabotaList rabotaList = new RabotaList();

            rabotaList.Rabotas = db.Rabota.ToList();

            return View(rabotaList);
        }
        public async Task<IActionResult> RabotaNaznach(RabotaRedact rabota)
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);




            db.Update(rabota.Rabot).State = EntityState.Modified;
            //db.Rabota.Update(AddRabota);

            await db.SaveChangesAsync().ConfigureAwait(false);
            if (rabota.Rabot.UrovenRabot == "Край")
            {
                DelNaznach(rabota.Rabot.Id);
            }

            return RedirectToAction("RabotaList");

        }

        public async Task<IActionResult> Variants(int idRabota)
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            Variants model = new Variants(idRabota, db);
            return View("Variants", model);
        }
        public async Task<IActionResult> Variant(int nVar, int idSpec)
        {
            //string login = HttpContext.User.Identity.Name;
            //Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            Variant variant = new Variant(idSpec, nVar, db);
            return View("Variant", variant);
        }

        public IActionResult RabotaDel(int[] Id)
        {
            for (int i = 0; i < Id.Length; i++)
            {
                Rabota rabota = new Rabota { Id = Id[i] };
                db.Rabota.Remove(rabota);
                DelNaznach(Id[i]);
            }
            db.SaveChanges();

            return Json("ok");
        }

        public void DelNaznach(int id)
        {
            List<NaznachMo> Naznac = db.NaznachMo.Where(x => x.IdRab == id).ToList();
            db.NaznachMo.RemoveRange(Naznac);
            List<NaznachOo> NaznacOO = db.NaznachOo.Where(x => x.IdRab == id).ToList();
            db.NaznachOo.RemoveRange(NaznacOO);
            db.SaveChanges();
        }


        public IActionResult NaznachOO(int[] naz, int id)
        {
            DelNaznach(id);
            for (int i = 0; i < naz.Length; i++)
            {
                NaznachOo naznachOo = new NaznachOo();
                naznachOo.IdOo = naz[i];
                naznachOo.IdRab = id;
                db.NaznachOo.Add(naznachOo);
            }
            db.SaveChanges();
            return Json("Ok");
        }

        public IActionResult NaznachMO(int[] naz, int id)
        {
            DelNaznach(id);
            for (int i = 0; i < naz.Length; i++)
            {
                NaznachMo naznachMo = new NaznachMo();
                naznachMo.IdMo = naz[i];
                naznachMo.IdRab = id;
                db.NaznachMo.Add(naznachMo);
            }
            db.SaveChanges();
            return Json("Ok");
        }
    }
}
