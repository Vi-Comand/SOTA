using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using SOTA.Models.Pages.Test;
using SOTA.Models.Pages.TestRaschet;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        SotaContext db;
        public TestController(SotaContext context)
        {
            db = context;
        }
        //public  IActionResult SaveOtvet(int id,string text)
        //{
        //    var login = HttpContext.User.Identity.Name;
        //    int idUser = db.Users.Where(p => p.Name == login).First().Id;

        //    SaveOtvUser save=new SaveOtvUser(id, text, db, idUser);

        //   return Json ("ok");
        //}
        public IActionResult KonecRabot(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            IRaschetBallsUser raschet = new RaschetBallsUser(db, idRabota, idUser);
            return RedirectToAction("ViewResultTest", new { idRabota });

        }
        public IActionResult SaveOtvet(int id, string text, int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;

            SaveOtvUser save = new SaveOtvUser(id, text, db, idUser, idRabota);

            return Json("ok");
        }
        public IActionResult ViewResultTest(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            ResultTest Result = new ResultTest(idUser, idRabota, db);
            int idspec = db.Rabota.First(x => x.Id == idRabota).IdSpec;
            double sumrab = db.Zadanie.Where(x => x.IdSpec == idspec).Sum(r => r.Ball);
            ViewBag.sumrab = sumrab;
            return View("ViewResultTest", Result);
        }
        public IActionResult Test(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            OpredelenieVariant Var = new OpredelenieVariant(idRabota, idUser, db);
            VarintTest Test = new VarintTest(idRabota, Var.GetVariant(), db, idUser);

            return View("Test", Test);
        }
    }
}
