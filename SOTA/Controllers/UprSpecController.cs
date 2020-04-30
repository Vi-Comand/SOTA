using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace SOTA.Controllers
{
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
        public async Task<IActionResult> SaveWysiwygText(string Name, string IdSpec, string Text, string Variant, string Nomer)
        {
            Zadanie AddZadanie = new Zadanie();
            AddZadanie.Name = Name;
            AddZadanie.IdSpec = Convert.ToInt32(IdSpec);
            AddZadanie.Text = Text;
            AddZadanie.Variant = Convert.ToInt32(Variant);
            AddZadanie.Nomer = Convert.ToInt32(Nomer);



            await db.Zadanie.AddAsync(AddZadanie).ConfigureAwait(false);
            await db.SaveChangesAsync().ConfigureAwait(false);
            return Json("ok");
        }

        public IActionResult SpecifikacList()
        {
            return View();
        }

        public IActionResult SpecifikacAdd()
        {
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
            ZadanVivod Zadan = new ZadanVivod();
            List<Otvet> ListOtv = new List<Otvet>();
            Zadan.Zadan = db.Zadanie.Find(1);
            try
            {
                if (Zadan.Zadan.Tip == 2)
                    Zadan.Otv = db.Otvet.Where(x => x.IdZadan == 1).ToList();
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
            List<Otvet> Otv = db.Otvet.Where(x => x.IdZadan == 1 && x.Param1 < 2).ToList();
            return Otv;
        }


        public async Task<IActionResult> SaveOtveti(int tip, int idZadania, string[] arr, string[] arr1, int obshBall)
        {
            List<Otvet> OldOtv = db.Otvet.Where(x => x.IdZadan == idZadania).ToList();
            db.Otvet.RemoveRange(OldOtv);
            await db.SaveChangesAsync().ConfigureAwait(false);
            Zadanie EditZadanie = new Zadanie();
            EditZadanie = db.Zadanie.Find(idZadania);
            EditZadanie.Tip = tip;

            if (obshBall == 0)
            {
                EditZadanie.Ball = Convert.ToDouble(arr1[0]);
            }
            else
            {
                EditZadanie.Ball = 0;
            }
            if (tip == 4)
            {
                List<Otvet> ListOtvets = new List<Otvet>();
                Otvet AddOtvet = new Otvet();
                for (int i = 0; i < arr.Count(); i++)
                {

                    if (i % 2 == 0)
                    {
                        AddOtvet = new Otvet();
                        AddOtvet.Param1 = Convert.ToDouble(arr[i]);
                        if (obshBall == 1 && AddOtvet.Param1 == 1.1)
                        {
                            AddOtvet.Ball = Convert.ToDouble(arr1[i]);
                        }


                    }
                    else
                    {
                        AddOtvet.IdZadan = idZadania;
                        AddOtvet.Text = arr[i];

                        ListOtvets.Add(AddOtvet);
                    }

                }
                await db.Otvet.AddRangeAsync(ListOtvets).ConfigureAwait(false);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                try
                {
                    List<Otvet> ListOtvets = new List<Otvet>();
                    for (int i = 0; i < arr.Count(); i++)
                    {
                        Otvet AddOtvet = new Otvet();
                        AddOtvet.IdZadan = idZadania;

                        if (tip == 2)
                        {

                            AddOtvet.Text = arr[i];



                            if (obshBall == 0)
                            {
                                AddOtvet.Ball = 0;
                                if (Convert.ToDouble(arr1[i]) != 0)
                                    AddOtvet.Verno = 1;
                            }
                            else
                            {
                                if (Convert.ToDouble(arr1[i]) > 0)
                                {
                                    AddOtvet.Ball = Convert.ToDouble(arr1[i]);
                                    AddOtvet.Verno = 1;
                                }
                                else
                                {
                                    AddOtvet.Ball = 0;
                                    AddOtvet.Verno = 0;
                                }
                            }

                        }
                        else
                        {
                            AddOtvet.Text = arr[i];
                            AddOtvet.Verno = 1;
                        }
                        if (tip == 5)
                        {
                            AddOtvet.Ball = Convert.ToDouble(arr1[i]);
                        }
                        ListOtvets.Add(AddOtvet);
                    }
                    await db.Otvet.AddRangeAsync(ListOtvets).ConfigureAwait(false);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                { return Json(ex); }
            }


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





        public IActionResult Zadanie(int n_var, int n_zad, int id_spec)
        {

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
