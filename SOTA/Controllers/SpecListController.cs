using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
