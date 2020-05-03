using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOTA.Models;

namespace SOTA.Controllers
{
    public class RabotaController : Controller
    {
        SotaContext db;

        public RabotaController(SotaContext context)
        {
            db = context;
        }
        public IActionResult RabotaAdd()
        {
            ViewData["Predms"] = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            ViewData["SpisSpec"] = db.Specific;
            return View();
        }

        [Route("Rabota/RabotaAdd/{IdRabota?}")]
        public IActionResult RabotaAdd(int IdRabota)
        {
            Rabota rabota = db.Rabota.Find(IdRabota);

            ViewData["Predms"] = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            ViewData["SpisSpec"] = db.Specific;
            return View(rabota);
        }
        public IActionResult RabotaList()
        {
            RabotaList rabotaList = new RabotaList();
            rabotaList.Predms = db.Predm.ToList();
            rabotaList.Rabotas = db.Rabota.ToList();
            rabotaList.Specs = db.Specific.ToList();
            return View(rabotaList);
        }
        public async Task<IActionResult> AddRabota(Rabota rabota)
        {
            Rabota AddRabota = new Rabota();
            AddRabota.Name = rabota.Name;
            AddRabota.IdSpec = Convert.ToInt32(rabota.IdSpec);
            AddRabota.Dliteln = Convert.ToInt32(rabota.Dliteln);
            AddRabota.UrovenRabot = Convert.ToInt32(rabota.UrovenRabot);
            AddRabota.Nachalo = Convert.ToDateTime(rabota.Nachalo);
            AddRabota.Konec = Convert.ToDateTime(rabota.Konec);
            AddRabota.ListUchasn = rabota.ListUchasn;
            if (rabota.Id == 0)
            {
                AddRabota.Sozd = DateTime.Now;
                await db.Rabota.AddAsync(AddRabota).ConfigureAwait(false);
            }
            else
            {
                AddRabota.Id = rabota.Id;
                db.Update(AddRabota).State = EntityState.Modified;
                //db.Rabota.Update(AddRabota);
            }
            await db.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction("RabotaList");
        }
    }
}