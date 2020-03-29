using Microsoft.AspNetCore.Mvc;
using Sota.Models;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


        public async Task<IActionResult> VivodZadaniaAjax(int idZadania)
        {
            ZadanVivod Zadan = new ZadanVivod();
            Zadan.Zadan= db.Zadanie.Find(idZadania);
            Zadan.Otv=db.Otvet.Where(x => x.IdZadan == idZadania).ToList();
            return Json(new { data = Zadan });
        }

            public async Task<IActionResult> SaveOtveti(int tip, int idZadania, string[] arr, string[] arr1, int obshBall)
        {
            List<Otvet> OldOtv = db.Otvet.Where(x => x.IdZadan == idZadania).ToList() ;
             db.Otvet.RemoveRange(OldOtv);
            await db.SaveChangesAsync().ConfigureAwait(false);
            Zadanie EditZadanie = new Zadanie();
           EditZadanie = db.Zadanie.Find(idZadania);
            EditZadanie.Tip = tip;

            if (obshBall == 0)
            {
                EditZadanie.Ball = Convert.ToDouble(arr1[0]);
            }
            if (tip == 4)
            {
                List<Otvet> ListOtvets = new List<Otvet>();
                Otvet AddOtvet = new Otvet(); 
                for (int i = 0; i < arr.Count(); i++)
                {
                  
                    if (i%2==0)
                    {
                        AddOtvet = new Otvet();
                        AddOtvet.Param1 = Convert.ToDouble(arr[i]);
                        if (obshBall == 1 && AddOtvet.Param1==1.1)
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
                           
                            

                                if (obshBall == 1)
                            {
                                AddOtvet.Ball = Convert.ToDouble(arr1[i]);
                               if (Convert.ToDouble(arr1[i])!=0)
                                AddOtvet.Verno = 1;
                            }                            
                            else
                            {
                                if(Convert.ToDouble(arr1[i])==1)
                                    { AddOtvet.Verno = 1; }
                                else
                                    { AddOtvet.Verno = 0; }
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

        public IActionResult Zadanie(string spec, string idZadan)
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
