﻿using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using SOTA.Models.Pages.Test;
using SOTA.Models.Pages.TestRaschet;
using System;
using System.Linq;
using System.Threading;

namespace SOTA.Controllers
{
    //[Authorize]
    public class TestController : Controller
    {
        SotaContext db;
        public TestController(SotaContext context)
        { int pr=1;
            db = context;
        }
        //public  IActionResult SaveOtvet(int id,string text)
        //{
        //    var login = HttpContext.User.Identity.Name;
        //    int idUser = db.Users.Where(p => p.Name == login).First().Id;

        //    SaveOtvUser save=new SaveOtvUser(id, text, db, idUser);

        //   return Json ("ok");
        //}
        public IActionResult KonecRabot(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            VariantUser variantUser = db.VariantUser.Where(x => x.IdUser == idUser && x.IdRabota == idRabota).First();
            Thread.Sleep(1000);

            if (variantUser.Konec == 0)
            {
                variantUser.Konec = 1;
                variantUser.KonecDate = DateTime.Now;

                IRaschetBallsUser raschet = new RaschetBallsUser(db, idRabota, idUser);


            }

            return RedirectToAction("ViewResultTest", new { idRabota });

        }





        //---------------------------------------------------тест расчета-------------------------------------------------------

        public void KonecRabot1(int idRabota, int id)
        {

            VariantUser variantUser = db.VariantUser.Where(x => x.IdUser == id && x.IdRabota == idRabota).First();

            variantUser.Konec = 1;
            variantUser.KonecDate = DateTime.Now;

            IRaschetBallsUser raschet = new RaschetBallsUser(db, idRabota, id);



        }




        public IActionResult TesRaschet()
        {
            return View();
        }


        public IActionResult TestRaschet(int id)
        {


            KonecRabot1(4, id);
            return Json("ok");

        }



        //-----------------------------------------------------------------------------------------------------------------



        public IActionResult ChekingSaveOtvet(int id, int idRabota, int index,string text)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            try
            {
                string textOtv = db.AnswerUser.Where(x => x.IdUser == idUser && x.IdRabota == idRabota && x.IdZadan == id).First().TextOtv;
                if (textOtv == text)
                    return Json(new { data = text, index });
                else
                {
                    SaveOtvUser save = new SaveOtvUser(id, text, db, idUser, idRabota, 1);
                 
                }
            }
            catch
            {
                SaveOtvUser save = new SaveOtvUser(id, text, db, idUser, idRabota, 1);
            }

            return Json("neok");
        }

        public IActionResult SaveOtvet(int id, string text, int idRabota, int proveren)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;

            SaveOtvUser save = new SaveOtvUser(id, text, db, idUser, idRabota, proveren);

            return Json("ok");
        }
        public IActionResult ViewResultTest(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            int idspec = db.Rabota.First(x => x.Id == idRabota).IdSpec;
            ResultTest Result = new ResultTest(idUser, idRabota, idspec, db);
            double sumrab = db.StructSpec.Where(x => x.IdSpec == idspec && x.Type == 1).Sum(r => Convert.ToDouble(r.Text));
           
            ViewBag.sumrab = sumrab;
            ViewBag.role = "0";
            ViewBag.dateK = db.Rabota.First(x => x.Id == idRabota).Konec;
            return View("ViewResultTest", Result);
        }
        public IActionResult Test(int idRabota)
        {
            var login = HttpContext.User.Identity.Name;
            int idUser = db.Users.Where(p => p.Name == login).First().Id;
            var Konec = db.VariantUser.Where(x => x.IdUser == idUser && x.IdRabota == idRabota).Select(x => x.Konec).FirstOrDefault();
            // int Konec = UserVariant !=null ? UserVariant.First().Konec:0;
            DateTime dateK = db.Rabota.Find(idRabota).Konec;
            if (Konec == 0 && DateTime.Now < dateK)
            {
                OpredelenieVariant Var = new OpredelenieVariant(idRabota, idUser, db);
                VarintTest Test = new VarintTest(idRabota, Var.GetVariant(), db, idUser);
                return View("Test", Test);
            }
            return RedirectToAction("ViewResultTest", new { idRabota });
        }
    }
}
