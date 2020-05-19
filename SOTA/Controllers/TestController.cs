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
        public IActionResult Test()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            return View();
        }
    }
}
