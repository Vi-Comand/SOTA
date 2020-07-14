using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System.Linq;
using System.Threading.Tasks;
using SOTA.Models.Pages.TestRaschet;

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
        public IActionResult KonecRabot( int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            IRaschetBallsUser raschet=new RaschetBallsUser(db, idRabota, idUser);
            return Json("ok");
        }
        public IActionResult SaveOtvet(int id, string text, int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;

            SaveOtvUser save = new SaveOtvUser(id, text, db, idUser,idRabota);

            return Json("ok");
        }
        public IActionResult Test(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            OpredelenieVariant Var=new OpredelenieVariant(idRabota,idUser,db);
          VarintTest Test= new VarintTest(idRabota, Var.GetVariant(),db);
          
            return View("Test",Test);
        }
    }
}
