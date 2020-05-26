using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SOTA.Controllers
{
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

        [Route("Rabota/RabotaAdd/{IdRabota?}")]
        //  [HttpPost]
        public IActionResult RabotaAdd(int IdRabota)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            RabotaList rabota = new RabotaList();
            rabota.Rabot = db.Rabota.Find(IdRabota);
            rabota.Oos = db.Oo.ToList();
            rabota.Mos = db.Mo.ToList();
            rabota.Predms = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            rabota.Specs = db.Specific.ToList();
            //if (error == "")
            //{
            return View(rabota);
            //}
            //else
            //{
            //    ViewData["Error"] = "В данной спецификации не заполнены все задания";
            //    return View(rabota);
            //}
        }
        public IActionResult RabotaList()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            RabotaList rabotaList = new RabotaList();
            rabotaList.Predms = db.Predm.ToList();
            rabotaList.Rabotas = db.Rabota.ToList();
            rabotaList.Specs = db.Specific.ToList();
            return View(rabotaList);
        }
        public async Task<IActionResult> AddRabota(RabotaList rabota)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            Rabota AddRabota = new Rabota();
            List<Zadanie> pustZadan = db.Zadanie.Where(x => x.IdSpec == rabota.Rabot.IdSpec && x.Text == null).ToList();

            if (pustZadan.Count == 0)
            {
                AddRabota.Name = rabota.Rabot.Name;
                AddRabota.IdSpec = Convert.ToInt32(rabota.Rabot.IdSpec);
                AddRabota.Dliteln = Convert.ToInt32(rabota.Rabot.Dliteln);
                AddRabota.UrovenRabot = rabota.Rabot.UrovenRabot;
                AddRabota.Nachalo = Convert.ToDateTime(rabota.Rabot.Nachalo);
                AddRabota.Konec = Convert.ToDateTime(rabota.Rabot.Konec);
                AddRabota.ListUchasn = rabota.Rabot.ListUchasn;
                AddRabota.Sozd = DateTime.Now;
                if (rabota.Rabot.Id == 0)
                {

                    await db.Rabota.AddAsync(AddRabota).ConfigureAwait(false);
                }
                else
                {

                    AddRabota.Id = rabota.Rabot.Id;
                    db.Update(AddRabota).State = EntityState.Modified;
                    //db.Rabota.Update(AddRabota);
                }

                await db.SaveChangesAsync().ConfigureAwait(false);
                error = "";
                return RedirectToAction("RabotaList");
            }
            else
            {
                error = "В данной спецификации не заполнены все задания";
                ViewData["Error"] = error;
                return RedirectPreserveMethod("RabotaAdd");
            }
        }

        public async Task<IActionResult> Variants(int idRabota)
        {
            Variants model = new Variants(idRabota, db);
            return View("Variants", model);
        }
        public async Task<IActionResult> Variant(int nVar, int idSpec)
        {
            Variant variant = new Variant(idSpec, nVar, db);


            return View("Variant", variant);
        }
    }
}
