using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOTA.Models;


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
            ViewData["Predms"] = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            ViewData["SpisSpec"] = db.Specific;
            return View();
        }

        [Route("Rabota/RabotaAdd/{IdRabota?}")]
        //  [HttpPost]
        public IActionResult RabotaAdd(int IdRabota)
        {
            Rabota rabota = db.Rabota.Find(IdRabota);

            ViewData["Predms"] = db.Predm.ToList();
            ViewData["TipSpecs"] = db.TipSpec.ToList();
            ViewData["SpisSpec"] = db.Specific;
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
            RabotaList rabotaList = new RabotaList();
            rabotaList.Predms = db.Predm.ToList();
            rabotaList.Rabotas = db.Rabota.ToList();
            rabotaList.Specs = db.Specific.ToList();
            return View(rabotaList);
        }

        public async Task<IActionResult> AddRabota(Rabota rabota)
        {
            Rabota AddRabota = new Rabota();
            List<Zadanie> pustZadan = db.Zadanie.Where(x => x.IdSpec == rabota.IdSpec && x.Text == null).ToList();

            if (pustZadan.Count == 0)
            {
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
             Variants model=new Variants(idRabota,db);
                return View("Variants",model);
            }
        public async Task<IActionResult> Variant(int nVar,int idSpec)
        {
            Variant variant = new Variant(idSpec,nVar,db);
           

            return View("Variant",variant);
        }
    }
    }
