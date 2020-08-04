using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            ViewData["Predms"] = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            ViewData["SpisSpec"] = db.Specific;
            return View();
        }

        //[Route("Rabota/RabotaAdd/{IdRabota?}")]
        //  [HttpPost]



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
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            RabotaRedact rabota = new RabotaRedact(db,IdRabota);
           
        
           

          

           // ViewData["TipSpecs"] = db.TipSpec.ToList();
          
          
            return View(rabota);
           
        }
        public IActionResult RabotaList()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            RabotaList rabotaList = new RabotaList();
          
            rabotaList.Rabotas = db.Rabota.ToList();
      
            return View(rabotaList);
        }
        public async Task<IActionResult> RabotaNaznach(RabotaRedact rabota)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            Rabota AddRabota = new Rabota();
            List<Zadanie> pustZadan = db.Zadanie.Where(x => x.IdSpec == rabota.Rabot.IdSpec && x.Text == null).ToList();

            AddRabota.Name = rabota.Rabot.Name;
            AddRabota.IdSpec = Convert.ToInt32(rabota.Rabot.IdSpec);
            AddRabota.Dliteln = Convert.ToInt32(rabota.Rabot.Dliteln);
            AddRabota.UrovenRabot = rabota.Rabot.UrovenRabot;
            AddRabota.Nachalo = Convert.ToDateTime(rabota.Rabot.Nachalo);
            AddRabota.Konec = Convert.ToDateTime(rabota.Rabot.Konec);
            AddRabota.ListUchasn = rabota.Rabot.ListUchasn;
            AddRabota.Sozd = DateTime.Now;
            AddRabota.Id = rabota.Rabot.Id;
            db.Update(AddRabota).State = EntityState.Modified;
            //db.Rabota.Update(AddRabota);

            await db.SaveChangesAsync().ConfigureAwait(false);

            error = "";
            return RedirectToAction("RabotaList");

        }

        public async Task<IActionResult> Variants(int idRabota)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            Variants model = new Variants(idRabota, db);
            return View("Variants", model);
        }
        public async Task<IActionResult> Variant(int nVar, int idSpec)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
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
