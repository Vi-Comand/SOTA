using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SOTA.Controllers
{
    [Authorize]
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
            //  Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = Convert.ToInt16(HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            ListUsersAdmin listUsersAdmin = new ListUsersAdmin(db);
            listUsersAdmin.LisrUsersA();
            UsersPage usersPage = new UsersPage();
            usersPage.LisrUsersMO = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 3).OrderBy(x => x.IdMo).ToList();
            usersPage.LisrUsersOO = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 2).OrderBy(x => x.IdMo).ToList();
            usersPage.LisrUsersKlass = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 1).OrderBy(x => x.IdMo).ThenBy(x => x.IdOo).ToList();
            // usersPage.LisrUsersTest = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 0).ToList();
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
            usersPage.LisrUsersKlass = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 1 && x.IdOo == IoO).OrderBy(x => x.Klass).ToList();
            usersPage.LisrUsersTest = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 0 && x.IdOo == IoO).OrderBy(x => x.Klass).ThenBy(x => x.F).ToList();
            return View("OO", usersPage);
        }

        /*    public IActionResult Klass()
            {
                string login = HttpContext.User.Identity.Name;
                Users user = db.Users.Where(p => p.Name == login).First();
                int ImKlass = user.IdKlass;
                ViewBag.rl = user.Role;
                ListUsersAdmin listUsersAdmin = new ListUsersAdmin(db);
                listUsersAdmin.GetRab(user.IdMo, user.IdOo, user.IdKlass);
                if (listUsersAdmin.rabList == null)
                {
                    listUsersAdmin.LisrUsersA();
                }
                else
                {
                    listUsersAdmin.LisrUsersA(listUsersAdmin.rabList.Last().Id);
                    ViewBag.last = listUsersAdmin.rabList.Last().Id;
                }
                UsersPage usersPage = new UsersPage();
                usersPage.rabotaKlasss = listUsersAdmin.rabList;
                usersPage.LisrUsersTest = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 0 && x.IdKlass == ImKlass).OrderBy(x => x.Klass).ToList();

                return View("Klass", usersPage);
            }*/

        public IActionResult Klass(UsersPage page)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            int ImKlass = user.IdKlass;
            ViewBag.rl = user.Role;
            ListUsersAdmin listUsersAdmin = new ListUsersAdmin(db);
            listUsersAdmin.GetRab(user.IdMo, user.IdOo, user.IdKlass);
            int id_spec=0;
                string name="";
            if (listUsersAdmin.rabList != null)
            {
                listUsersAdmin.rabList.OrderBy(x => x.Konec);
            
             id_spec = listUsersAdmin.rabList.OrderByDescending(x => x.Konec).First().Id;
             name = listUsersAdmin.rabList.OrderByDescending(x => x.Konec).First().Name;
            }
            if (listUsersAdmin.rabList == null)
            {
                listUsersAdmin.LisrUsersA();
            }
            else
            {

                listUsersAdmin.LisrUsersA(id_spec);
                ViewBag.last = name;
            }
            UsersPage usersPage = new UsersPage();
            usersPage.rabotaKlasss = listUsersAdmin.rabList;
            usersPage.LisrUsersTest = listUsersAdmin.LisrUsersAdm.Where(x => x.Role == 0 && x.IdKlass == ImKlass).OrderBy(x => x.Klass).ToList();

            return View("Klass", usersPage);
        }


        public IActionResult CleanPass(int idDel, int role)
        {
            if (role == 4)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idDel);
                user.Pass = null;
                user.DateReg = DateTime.MinValue;
                db.SaveChanges();
                return Redirect("Users");
            }
            if (role == 3)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idDel);
                user.Pass = null;
                user.DateReg = DateTime.MinValue;
                db.SaveChanges();
                return Redirect("UO");
            }
            if (role == 2)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idDel);
                user.Pass = null;
                user.DateReg = DateTime.MinValue;
                db.SaveChanges();
                return Redirect("OO");
            }
            if (role == 1)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idDel);
                user.Pass = null;
                user.DateReg = DateTime.MinValue;
                db.SaveChanges();
                return Redirect("Klass");
            }
            else
            {
                return Redirect("Index");
            }
        }

        public IActionResult AddUserAjax(string f, string i, string o)
        {
            string login = HttpContext.User.Identity.Name;
            Users klass = db.Users.Where(p => p.Name == login).First();
            Users user = new Users();
            string n="";
            var list = db.Users.Select(x => new   {name = x.Name }).ToList();
               
                user.F = f;
                user.I = i;
                user.O = o;
            n = f + i + o;
            for (int j = 1; j < 1000; j++)
            {
                var q = list.Where(x => x.name.ToString() == n).Count();
                
                
                if (q == 0)
                {
                    user.Name = n;
                    break;
                }
                else
                {
                    n = f + i + o;
                    n = n +"_"+j.ToString();
                 }
            }
                user.IdKlass = klass.IdKlass;
                user.IdMo = klass.IdMo;
                user.IdOo = klass.IdOo;
                user.Kod = "1";

            db.Users.Add(user);
                db.SaveChanges();
                return Json("Ok");
         
        }


        public IActionResult DelUser(int idDel)
        {
            string login = HttpContext.User.Identity.Name;
            Users klass = db.Users.Where(p => p.Name == login).First();
            if (klass.Role == 1)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idDel);
                UsersDeleted usersDeleted = new UsersDeleted();
                usersDeleted.DateDel = DateTime.Now;
                usersDeleted.F = user.F;
                usersDeleted.I = user.I;
                usersDeleted.O = user.O;
                usersDeleted.Name = user.Name;
                usersDeleted.IdKlass = user.IdKlass;
                usersDeleted.IdMo = user.IdMo;
                usersDeleted.IdOo = user.IdOo;
                usersDeleted.IdDel = user.Id;
                db.UsersDeleted.Add(usersDeleted);
                db.Users.Remove(user);
                db.SaveChanges();
                return Redirect("Klass");
            }
            else
            {
                return Redirect("Index");
            }
        }

            public IActionResult RazrReg(int idReg, int role)
        {
            if (role == 4)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idReg);
                user.Kod = "0";
                db.SaveChanges();
                return Redirect("Users");
            }
            if (role == 3)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idReg);
                user.Kod = "0";
                db.SaveChanges();
                return Redirect("UO");
            }
            if (role == 2)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idReg);
                user.Kod = "0";
                db.SaveChanges();
                return Redirect("OO");
            }
            if (role == 1)
            {
                Users user = new Users();
                user = db.Users.FirstOrDefault(u => u.Id == idReg);
                user.Kod = "0";
                db.SaveChanges();
                return Redirect("Klass");
            }
            else
            {
                return Redirect("Index");
            }
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
