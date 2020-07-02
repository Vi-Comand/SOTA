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
            ListUsersAdmin listUsersAdmin = new ListUsersAdmin(db);
            listUsersAdmin.LisrUsersA();
            UsersPage usersPage = new UsersPage();
            usersPage.LisrUsersMO = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 3).OrderBy(x => x.IdMo).ToList();
            usersPage.LisrUsersOO = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 2).OrderBy(x => x.IdMo).ToList();
            usersPage.LisrUsersKlass = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 1).OrderBy(x => x.IdMo).ThenBy(x => x.IdOo).ToList();
            usersPage.LisrUsersTest = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 0).ToList();
            // LisrUsersA listMo = new ListUsersMo();
            //listMo.UsersMo = db.Users.Where(p => p.Role == 3).ToList();
            return View("Users", usersPage);
        }

        public IActionResult UO()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            int ImO = user.IdMo;
            ViewBag.rl = user.Role;
            ListUsersAdmin listUsersAdmin = new ListUsersAdmin(db);
            listUsersAdmin.LisrUsersA();
            UsersPage usersPage = new UsersPage();
            usersPage.LisrUsersOO = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 2 && x.IdMo == ImO).OrderBy(x => x.IdMo).ToList();
            return View("UO", usersPage);
        }

        public IActionResult OO()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            int IoO = user.IdOo;
            ViewBag.rl = user.Role;
            ListUsersAdmin listUsersAdmin = new ListUsersAdmin(db);
            listUsersAdmin.LisrUsersA();
            UsersPage usersPage = new UsersPage();
            usersPage.LisrUsersKlass = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 1 && x.IdOo == IoO).OrderBy(x => x.IdOo).ToList();
            return View("OO", usersPage);
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

            return Redirect("Users");
        }

    }
}
