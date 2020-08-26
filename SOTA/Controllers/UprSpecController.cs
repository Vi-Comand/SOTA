using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;




namespace SOTA.Controllers
{
    [Authorize]
    public class UprSpec : Controller
    {

        SotaContext db;

        public UprSpec(SotaContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveWysiwygText(int idZadania, string Text)
        {
            Zadanie RedactZandan = db.Zadanie.Find(idZadania);

            if (Text != RedactZandan.Text)
            {
                RedactZandan.Text = Text;


                await db.SaveChangesAsync().ConfigureAwait(false);
                return Json("Изменения внесены");
            }
            else
            {
                return Json("Изменения нет");
            }

            return Json("ok");
        }

        public IActionResult SpecifikacList()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            return View();
        }

        public IActionResult SpecifikacAdd()
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            SpecifikacAddModel model = new SpecifikacAddModel();
            model.Spec = new Specific();
            model.Predms = db.Predm.ToList();
            model.TipSpecs = db.TipSpec.ToList();
            return View(model);
        }


        public IActionResult SpecifikacCreate(SpecifikacAddModel model)
        {
            db.Specific.Add(model.Spec);
            db.SaveChanges();
            return RedirectToAction("SpecifikacRedact", new { id_spec = model.Spec.Id });
        }

        public IActionResult SpecifikacIzmen(SpecifikacAddModel model)
        {
            db.Specific.Update(model.Spec);
            db.SaveChanges();
            return RedirectToAction("SpecifikacList", "SpecList");
        }

        public IActionResult IzmKolZadans(SpecifikacRedactModel model)
        {
            int n_spec = model.Spec.Id;
            int kol_var = model.KolVar;
            int kol_zad = model.KolZad;

            int KolZadansVVar = !db.Zadanie.Any(x => x.Variant == 1 && x.IdSpec == n_spec) ? 0 : db.Zadanie.Count(x => x.Variant == 1 && x.IdSpec == n_spec);

            int KolVar = !db.Zadanie.Any(x => x.IdSpec == n_spec) ? 0 : db.Zadanie.Where(x => x.IdSpec == n_spec).OrderByDescending(x => x.Variant).First().Variant;
            if (KolZadansVVar != 0 && KolVar != 0)
            {
                if (KolVar < kol_var)
                {
                    List<Zadanie> AddZadans = new List<Zadanie>();
                    for (int i = KolVar + 1; i <= kol_var; i++)
                    {
                        AddZadans.AddRange(AddedVar(i, kol_zad, n_spec));
                    }
                    db.Zadanie.AddRange(AddZadans);
                    db.SaveChanges();
                }
                if (KolZadansVVar < kol_zad)
                {
                    List<Zadanie> AddZadans = new List<Zadanie>();
                    for (int i = 1; i <= kol_var; i++)
                    {
                        AddZadans.AddRange(AddedVar(i, kol_zad, n_spec, KolZadansVVar + 1));
                    }
                    db.Zadanie.AddRange(AddZadans);
                    db.SaveChanges();
                }
                if (KolZadansVVar > kol_zad)
                {
                    db.Zadanie.RemoveRange(db.Zadanie.Where(x => x.IdSpec == n_spec && x.Nomer > kol_zad));
                    db.SaveChanges();
                }
                if (KolVar > kol_var)
                {
                    db.Zadanie.RemoveRange(db.Zadanie.Where(x => x.IdSpec == n_spec && x.Variant > kol_var));
                    db.SaveChanges();
                }
            }
            else
            {
                List<Zadanie> AddZadans = new List<Zadanie>();
                for (int i = 1; i <= kol_var; i++)
                {
                    AddZadans.AddRange(AddedVar(i, kol_zad, n_spec));
                }
                db.Zadanie.AddRange(AddZadans);
                db.SaveChanges();
            }

            return RedirectToAction("SpecifikacRedact", new { id_spec = model.Spec.Id });
        }
        private List<Zadanie> AddedVar(int n_var, int kol_zad, int n_spec, int n_zad)
        {
            List<Zadanie> Variant = new List<Zadanie>();
            for (int j = n_zad; j <= kol_zad; j++)
            {

                Variant.Add(AddedZadan(n_var, j, n_spec));

            }
            return Variant;

        }
        private List<Zadanie> AddedVar(int n_var, int kol_zad, int n_spec)
        {
            List<Zadanie> Variant = new List<Zadanie>();
            for (int j = 1; j <= kol_zad; j++)
            {

                Variant.Add(AddedZadan(n_var, j, n_spec));

            }
            return Variant;

        }
        private Zadanie AddedZadan(int n_var, int n_zad, int n_spec)
        {
            Zadanie zadan = new Zadanie();
            zadan.IdSpec = n_spec;
            zadan.Variant = n_var;
            zadan.Ball = 1;
            zadan.Nomer = n_zad;

            return zadan;
        }
        public async Task<IActionResult> ChangedBallAjax(int n_spec, int n_zad, int ball)
        {

            List<Zadanie> Zadans = db.Zadanie.Where(t => t.IdSpec == n_spec && t.Nomer == n_zad).ToList();
            foreach (Zadanie row in Zadans)
                row.Ball = ball;
            db.SaveChanges();
            return Json("ok");
        }
        public async Task<IActionResult> VivodZadaniaAjax(int idZadania)
        {
            System.Threading.Thread.Sleep(100);
            ZadanVivod Zadan = new ZadanVivod();
            List<Otvet> ListOtv = new List<Otvet>();
            Zadan.Zadan = db.Zadanie.Find(idZadania);
            try
            {
                if (Zadan.Zadan.Tip == 2)
                    Zadan.Otv = db.Otvet.Where(x => x.IdZadan == idZadania && x.Ustar != 1).ToList();
                if (Zadan.Zadan.Tip == 4)
                    Zadan.Otv = ListTableOtv(idZadania);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex });
            }
            return Json(Zadan);
        }
        public List<Otvet> ListTableOtv(int idZadan)
        {
            List<Otvet> AllOtv = db.Otvet.Where(x => x.IdZadan == idZadan && x.Ustar != 1).ToList();
            List<Otvet> Otv = new List<Otvet>();
            foreach (var row in AllOtv)
            {
                double val = Math.Round(row.Param1 - (int)row.Param1, 2);
                if (val == 0.1)
                {
                    Otv.Add(row);
                }

            }

            return Otv;
        }

        private bool NeNuzniStarOtv(int idSpec)
        {
            List<Rabota> SpisokRabot = db.Rabota.Where(x => x.IdSpec == idSpec).ToList();
            bool UdalitStarOtv = true;
            foreach (Rabota rabota in SpisokRabot)
                if (rabota.Nachalo <= DateTime.Now.Date)
                {
                    UdalitStarOtv = false;
                    break;
                }

            return UdalitStarOtv;




        }
        private void UdalitStarOtv(int idZadania)
        {
            List<Otvet> OldOtv = db.Otvet.Where(x => x.IdZadan == idZadania).ToList();
            db.Otvet.RemoveRange(OldOtv);
            db.SaveChanges();
        }
        #region IzmenenjaOtvets
        private List<Otvet> PoiskIzmen(List<Otvet> UserOtvets, int idZadania, int tip)
        {
            var OtvVBD = db.Otvet.Where(x => x.IdZadan == idZadania && x.Ustar != 1);
            if (OtvVBD != null)
            {
                if (tip == 1)
                    IzmenOtvTip1(OtvVBD, UserOtvets);
                if (tip == 2)
                    IzmenOtvTip2(OtvVBD, UserOtvets);
                if (tip == 4)
                    IzmenOtvTip4(OtvVBD, UserOtvets);
                if (tip == 5)
                    IzmenOtvTip5(OtvVBD, UserOtvets);
            }
            return UserOtvets;
        }

        private List<Otvet> IzmenOtvTip1(IQueryable<Otvet> OtvVBD, List<Otvet> UserOtvets)
        {
            foreach (var otv in OtvVBD)
            {
                var EstOtv = UserOtvets.Where(x => x.Text == otv.Text).FirstOrDefault();
                if (EstOtv == null)
                {
                    otv.Ustar = 1;
                    otv.DataIzm = DateTime.Now;
                }
                UserOtvets.Remove(EstOtv);

            }
            db.UpdateRange(OtvVBD);
            db.SaveChanges();

            return UserOtvets;

        }

        private List<Otvet> IzmenOtvTip2(IQueryable<Otvet> OtvVBD, List<Otvet> userOtvets)
        {
            foreach (var otv in OtvVBD)
            {
                var EstOtv = userOtvets.FirstOrDefault(x => x.Text == otv.Text);
                if (EstOtv == null)
                {
                    otv.Ustar = 1;
                    otv.DataIzm = DateTime.Now;
                }
                else
                {
                    otv.Verno = EstOtv.Verno;
                    otv.Ball = EstOtv.Ball;
                }
                userOtvets.Remove(EstOtv);

            }
            db.UpdateRange(OtvVBD);
            db.SaveChanges();

            return userOtvets;

        }

        private List<Otvet> IzmenOtvTip4(IQueryable<Otvet> OtvVBD, List<Otvet> userOtvets)
        {
            foreach (var otv in OtvVBD)
            {
                var EstOtv = userOtvets.FirstOrDefault(x => x.Text == otv.Text);
                if (EstOtv == null)
                {
                    otv.Ustar = 1;
                    otv.DataIzm = DateTime.Now;
                }
                else
                {
                    otv.Param1 = EstOtv.Param1;
                    otv.Ball = EstOtv.Ball;
                    otv.DataIzm = DateTime.Now;
                }
                userOtvets.Remove(EstOtv);

            }
            db.UpdateRange(OtvVBD);
            db.SaveChanges();

            return userOtvets;

        }
        private List<Otvet> IzmenOtvTip5(IQueryable<Otvet> OtvVBD, List<Otvet> UserOtvets)
        {
            foreach (var otv in OtvVBD)
            {
                var EstOtv = UserOtvets.Where(x => x.Text == otv.Text).FirstOrDefault();
                if (EstOtv == null)
                {
                    otv.Ustar = 1;
                    otv.DataIzm = DateTime.Now;
                }
                else
                {

                    otv.Ball = EstOtv.Ball;
                    otv.DataIzm = DateTime.Now;
                }
                UserOtvets.Remove(EstOtv);

            }
            db.UpdateRange(OtvVBD);
            db.SaveChanges();

            return UserOtvets;

        }
        #endregion

        #region FomirListOtvetov
        private List<Otvet> FomirListOtv(int tip, int idZadania, string[] arr, string[] arr1, int obshBall)
        {
            List<Otvet> Otvets = new List<Otvet>();
            if (tip == 1)
                Otvets = OtvTip1(arr, idZadania);
            if (tip == 2)
                Otvets = OtvTip2(arr, arr1, idZadania, obshBall);
            if (tip == 4)
                Otvets = OtvTip4(arr, arr1, idZadania, obshBall);
            if (tip == 5)
                Otvets = OtvTip5(arr, arr1, idZadania);

            return Otvets;
        }

        private List<Otvet> OtvTip1(string[] arr, int idZadania)
        {
            List<Otvet> Otvets = new List<Otvet>();
            foreach (var row in arr)
            {
                Otvet Otv = new Otvet();
                Otv.IdZadan = idZadania;
                Otv.Verno = 1;
                Otv.Text = row;
                Otv.DataIzm = DateTime.Now;
                Otvets.Add(Otv);

            }
            return Otvets;
        }


        private List<Otvet> OtvTip2(string[] arr, string[] arr1, int idZadania, int obshBall)
        {
            List<Otvet> Otvets = new List<Otvet>();

            for (int i = 0; i < arr.Length; i++)
            {
                Otvet Otv = new Otvet();
                Otv.IdZadan = idZadania;
                if (Convert.ToInt32(arr1[i]) > 0)
                {
                    if (obshBall == 0)
                        Otv.Ball = Convert.ToInt32(arr1[i]);

                    Otv.Verno = 1;
                }
                else
                    Otv.Verno = 0;
                Otv.Text = arr[i];
                Otv.DataIzm = DateTime.Now;
                Otvets.Add(Otv);

            }

            //Otvets.Reverse();
            return Otvets;
        }
        private List<Otvet> OtvTip4(string[] arr, string[] arr1, int idZadania, int obshBall)
        {
            List<Otvet> Otvets = new List<Otvet>();
            Otvet Otv = new Otvet();
            int j = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Otv = new Otvet();
                    Otv.Param1 = Convert.ToDouble(arr[i]);
                    //if (obshBall == 1 && Otv.Param1 == 1.1)
                    //{
                    //    Otv.Ball = Convert.ToDouble(arr1[i]);
                    //}


                    if (obshBall == 0 && Math.Round(Otv.Param1 - (int)Otv.Param1, 1) == 0.1)
                    {
                        Otv.Ball = Convert.ToDouble(arr1[j]);
                        j++;
                    }

                }
                else
                {
                    Otv.IdZadan = idZadania;
                    Otv.Text = arr[i];
                    Otv.Verno = 1;
                    Otv.DataIzm = DateTime.Now;
                    Otvets.Add(Otv);
                }

            }
            return Otvets;
        }

        private List<Otvet> OtvTip5(string[] arr, string[] arr1, int idZadania)
        {

            List<Otvet> Otvets = new List<Otvet>();
            for (int i = 0; i < arr.Length; i++)
            {
                Otvet Otv = new Otvet();
                Otv.IdZadan = idZadania;
                Otv.Verno = 1;
                Otv.Text = arr[i];
                Otv.Ball = Convert.ToDouble(arr1[i]);
                Otv.DataIzm = DateTime.Now;
                Otvets.Add(Otv);

            }
            Otvets.Reverse();
            return Otvets;
        }

        #endregion
        public async Task<IActionResult> SaveOtveti(int tip, int idZadania, string[] arr, string[] arr1, int obshBall)
        {
            List<Otvet> ListOtvets = new List<Otvet>();

            Zadanie EditZadanie = new Zadanie();
            EditZadanie = db.Zadanie.Find(idZadania);
            EditZadanie.Tip = tip;
            if (NeNuzniStarOtv(EditZadanie.IdSpec))
            {
                UdalitStarOtv(idZadania);
                ListOtvets = FomirListOtv(tip, idZadania, arr, arr1, obshBall);

            }
            else
            {
                ListOtvets = PoiskIzmen(FomirListOtv(tip, idZadania, arr, arr1, obshBall), idZadania, tip);

            }

            db.Otvet.AddRangeAsync(ListOtvets).ConfigureAwait(false);
            db.SaveChanges();


            return Json("ok");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult Zadanie()
        //{
        //    return View();
        //}
        public IActionResult SpecifikacRedact(int id_spec)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            SpecifikacRedactModel model = new SpecifikacRedactModel();
            model.Spec = db.Specific.Find(id_spec);
            model.KolZad = !db.Zadanie.Any(x => x.Variant == 1) ? 0 : db.Zadanie.Count(x => x.Variant == 1 && x.IdSpec == id_spec);

            model.KolVar = !db.Zadanie.Any(x => x.IdSpec == id_spec) ? 0 : db.Zadanie.Where(x => x.IdSpec == id_spec).OrderByDescending(x => x.Variant).First().Variant;
            model.Zadanies = ProverkaNaOdinakovBall(db.Zadanie.Where(x => x.IdSpec == id_spec).ToList(), model.KolZad);

            model.Predms = db.Predm.ToList();
            model.TipSpecs = db.TipSpec.ToList();




            return View("SpecifikacRedact", model);
        }
        public List<Zadanie> ProverkaNaOdinakovBall(List<Zadanie> Zadans, int kol_zad)
        {
            List<Zadanie> Proveren = new List<Zadanie>();

            for (int i = 1; i <= kol_zad; i++)
            {
                List<Zadanie> NaProverku = Zadans.Where(x => x.Nomer == i).ToList();
                Proveren.AddRange(ZadanNaOdinakovBall(NaProverku));


            }

            return Proveren;
        }

        public async Task<List<Zadanie>> ZadanNaOdinakovBallAsync(List<Zadanie> Zadans)
        {
            var result = await Task.Run(() => ZadanNaOdinakovBall(Zadans));
            return result;
        }

        private List<Zadanie> ZadanNaOdinakovBall(List<Zadanie> Zadans)
        {
            bool NeOdinakov = false;
            if (Zadans != null)
            {
                double Ball = Zadans.First().Ball;
                foreach (Zadanie zadan in Zadans)
                {
                    if (zadan.Ball != Ball)
                    {
                        NeOdinakov = true;
                        break;
                    }
                    //Сомнительный костыль
                    if (zadan.Text != null)
                    {
                        zadan.Text = "1";
                    }
                }

            }
            if (NeOdinakov)
            {
                foreach (Zadanie zadan in Zadans)
                {
                    zadan.Ball = 0;
                }
            }

            return Zadans;
        }




        //[HttpPost]
        public async Task<IActionResult> LoadNaServAjaxTo(IList<IFormFile> files)
        {
            int lastId = db.SaveImg.Max(x => x.Id) + 1;
            string data = "";
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureCorrectFilename(filename);
                string tip = "." + filename.Substring(filename.LastIndexOf(".") + 1);

                filename = lastId.ToString() + tip;
                SaveImg saveImg = new SaveImg();
                saveImg.Name = lastId.ToString();
                saveImg.Tip = tip;
                db.SaveImg.Add(saveImg);
                db.SaveChanges();
                using (var fileStream = new FileStream(Directory.GetCurrentDirectory() + "//wwwroot//Img//" + filename, FileMode.Create))
                {
                    await source.CopyToAsync(fileStream).ConfigureAwait(false);
                }
                data = "http://" + Request.Host.ToUriComponent() + "//Img//" + filename;
            }

            return Json(data);
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }



        public IActionResult Zadanie(int n_var, int n_zad, int id_spec)
        {
            string login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            ZadanieRedact model = new ZadanieRedact();
            var Zadan = db.Zadanie.Where(x => x.IdSpec == id_spec && x.Nomer == n_zad && x.Variant == n_var).First();
            model.Id = Zadan.Id;
            model.IdSpec = id_spec;
            model.Text = Zadan.Text;
            model.Tip = Zadan.Tip;
            model.Ball = Zadan.Ball;
            model.Otvets = db.Otvet.Where(x => x.IdZadan == Zadan.Id && x.Ustar != 1).ToList();
            if (Zadan.Tip == 4 && model.Otvets != null)
                try
                {
                    model.KolStrTabOtv = (int)model.Otvets.Max(x => x.Param1);
                }
                catch
                { }
            return View("Zadanie", model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
