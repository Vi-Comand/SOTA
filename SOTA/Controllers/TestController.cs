using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
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
        public async Task<IActionResult> SaveOtvet(int id, string text)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;

            SaveOtvUser save = new SaveOtvUser(id, text, db, idUser);







            return Json("ok");
        }
        public IActionResult Test(int idRabota)
        {

            VarintTest Test = new VarintTest(idRabota, 1, db);

            return View("Test", Test);
        }
    }
}
