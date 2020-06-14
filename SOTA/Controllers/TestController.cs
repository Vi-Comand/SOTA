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
        public IActionResult Test(int idRabota)
        {
         
          VarintTest Test= new VarintTest(idRabota, 1,db);
          
            return View("Test",Test);
        }
    }
}
