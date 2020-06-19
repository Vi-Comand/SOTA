using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System.Linq;
namespace SOTA.Controllers
{
    public class TestController : Controller
    {
        SotaContext db;
        public TestController(SotaContext context)
        {
            db = context;
        }
        public async IActionResult SaveOtvet(int id,string text)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id; 
            
            return Json ("ok");
        }
            public IActionResult Test(int idRabota)
        {
         
          VarintTest Test= new VarintTest(idRabota, 1,db);
          
            return View("Test",Test);
        }
    }
}
