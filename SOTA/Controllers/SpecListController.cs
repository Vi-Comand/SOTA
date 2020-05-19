using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System.Linq;



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
    }
}
