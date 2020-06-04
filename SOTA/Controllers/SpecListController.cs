using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Controllers
{
    public class SpecList : Controller
    {
        SotaContext db;

        public SpecList(SotaContext context)
        {
            db = context;
        }
        public IActionResult SpecifikacList()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            SpecifikacsList model = new SpecifikacsList();

            model.Spec = db.Specific.ToList();
            model.Predms = db.Predm.ToList();
            SchetchikZadVSpecs schet = new SchetchikZadVSpecs();
            model.KolZadVSpec = schet.Poschitat(model.Spec.Select(x => x.Id).ToList(), db);
            model.TipSpecs = db.TipSpec.ToList();
            return View("SpecifikacList", model);
        }

        public async Task<IActionResult> SpecDel(int[] Id)
        {

            for (int i = 0; i < Id.Length; i++)
            {
                Specific specific = new Specific { Id = Id[i] };
                 db.Specific.Remove(specific);
                var zadanie = db.Zadanie.Where(p => p.IdSpec == Id[i]).ToList();
                // int[] ZadansListDel = zadanie.Id.;
                for (int j = 0; j < zadanie.Count; j++)
                {
                    try
                    {
                        //Zadanie zad = new Zadanie { Id = zadanie[i].Id };
                        db.Zadanie.Remove(zadanie[j]);

                        var otvet = db.Otvet.Where(p => p.IdZadan == zadanie[i].Id).ToList();
                        for (int k = 0; k < otvet.Count; k++)
                        {
                            
                            db.Otvet.Remove(otvet[k]);
                        }
                    }
                    catch (Exception ex) { string q = ex.Message; }
                }

            }
            db.SaveChanges();
            return RedirectToRoute("RabotaList");
        }
    }
}
