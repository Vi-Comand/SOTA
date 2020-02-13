using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sota.Models;

namespace SOTA.Controllers
{
    public class UprSpec : Controller
    {
        SotaContext db;

        public UprSpec(SotaContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SaveWysiwygText(string Name, string IdSpec, string Text  , string Variant , string Nomer)
        {
            Zadanie AddZadanie = new Zadanie();
            AddZadanie.Name = Name;
            AddZadanie.IdSpec = Convert.ToInt32(IdSpec);
            AddZadanie.Text = Text;
            AddZadanie.Variant = Convert.ToInt32(Variant);
            AddZadanie.Nomer = Convert.ToInt32(Nomer);
            


            await db.Zadanie.AddAsync(AddZadanie).ConfigureAwait(false);
            await db.SaveChangesAsync().ConfigureAwait(false);
            return Json("ok");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Zadanie()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
