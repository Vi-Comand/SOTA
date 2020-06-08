using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> LoadUsers(IList<IFormFile> uploadedFile)
        {



            if (uploadedFile != null)
            {
                foreach (var file in uploadedFile)
                {
                    ReadExcel readExcel = new ReadExcel(file);
                    var _ListExcel = readExcel.ListExcel();
                    LoadVBD loadVBD = new LoadVBD(_ListExcel, db);
                    loadVBD.CreateMO();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
