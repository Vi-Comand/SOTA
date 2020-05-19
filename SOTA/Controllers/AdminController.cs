using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;

namespace SOTA.Controllers
{
    public class AdminController : Controller
    {
        private SotaContext db;
        public AdminController(SotaContext context)
        {
            db = context;
        }

        public IActionResult Users()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            ListMo listMo = new ListMo();
            listMo.ListUsersMo = db.Users.Where(p => p.Role == 1).ToList();
            return View("Users", listMo);
        }

        public IActionResult CleanPass(int idDel)
        {
            Users user = new Users();
            user = db.Users.FirstOrDefault(u => u.Id == idDel);
            user.Pass = null;
            user.DateReg = DateTime.MinValue;
            db.SaveChanges();
            return Redirect("Users");
        }
    }
}
