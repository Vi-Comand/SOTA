using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;

namespace SOTA.Controllers
{
    [Authorize]
    public class ResponsChceck : Controller
    {
        SotaContext db;

        public ResponsChceck(SotaContext context)
        {
            db = context;
        }


        public IActionResult SelectRespons()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            SelectRespons selectRespons = new SelectRespons(db);
            ListRab listRab = new ListRab();
            listRab.ListRabs = selectRespons.LRabots();
            return View("SelectRespons", listRab);
        }

        public IActionResult SelectZadan(int idSpec)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            SelectRespons selectRespons = new SelectRespons(db);
            ListRab listRab = new ListRab();
            listRab.ListZads = selectRespons.LZadans(idSpec);
            var zadanies = listRab.ListZads.Select(x => x.IdZad).ToArray();
            var l = db.AnswerUser.Where(x => x.Proveren == 0 && zadanies.Contains(x.IdZadan)).ToList();

            if (l.Count == 0)
            {
                return View("Ok");
            }
            else
            {
                return View("SelectZadan", listRab);
            }

        }

        public IActionResult Proverka(int idZad)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            SelectRespons selectRespons = new SelectRespons(db);
            ListRab listRab = new ListRab();
            listRab.listKriters = selectRespons.LKriter(idZad);
            ViewBag.sp = db.Zadanie.Find(idZad).IdSpec.ToString();
            return View("CheckRespons", listRab);
        }

        public IActionResult GetTO(int id_zad)
        {
            AnswerUser answerUser = new AnswerUser();

            string TextZ = db.Zadanie.Find(id_zad).Text.First().ToString();
            string TextO;
            string Id_ans;
            string Id_rab;
            string Id_user;
            try
            {
                answerUser = db.AnswerUser.Where(x => x.IdZadan == id_zad && x.Proveren == 0).First();
                TextO = answerUser.TextOtv;
                Id_ans = answerUser.Id.ToString();
                Id_rab = answerUser.IdRabota.ToString();
                Id_user = answerUser.IdUser.ToString();
            }
            catch
            {
                TextO = "Всё проверено";
                Id_ans = "0";
                Id_rab = "0";
                Id_user = "0";
            }
            string[] data = new string[5] { TextZ, TextO, Id_ans, Id_rab, Id_user };
            return Json(data);
        }

        public IActionResult SaveK(int id_zad, double radio, int Id_ans, int Id_rab, int Id_user)
        {
            AnswerUser answerUser = new AnswerUser();
            answerUser = db.AnswerUser.Find(Id_ans);
            answerUser.Proveren = 1;
            db.Update(answerUser);
            db.SaveChanges();

            UsersBalls usersBalls = new UsersBalls { IdRabota = Id_rab, IdUser = Id_user, Ball = radio, IdZadania = id_zad, Date = DateTime.Now };
            db.Update(usersBalls);
            db.SaveChanges();

            return Json("OK");
        }
    }
}
