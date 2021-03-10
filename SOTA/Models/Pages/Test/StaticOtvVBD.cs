using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Test
{
    public static class StaticRabotsOtvVBD
    {
       public static List<StaticZadOtvVBD> staticOtvVBDs { get; }

        static StaticRabotsOtvVBD()
        {
            staticOtvVBDs = new List<StaticZadOtvVBD>();
           
        }
        public static void Add(int IDSpec, SotaContext db)
        {
            var add = new StaticZadOtvVBD(db, IDSpec);
            add.Add();

            staticOtvVBDs.Add(add);
        }
        public static bool Find(int idSpec)
        {
            if (staticOtvVBDs.Where(x => x.IDSpec == idSpec).Any())
                return true;
                return false;
        }
        //public static List<StaticZadOtvVBD> SearchZadans(int[] idZadans,int idSpec)
        //{
        //    return staticOtvVBDs.Where(x=>x.IDSpec==idSpec).Select(y => y.ZadOtvVBD);
        //}

        public static void CleanStaticOtvVBDs()
        {if(staticOtvVBDs.Any())
            staticOtvVBDs.Clear();
        }
    }

    public class StaticZadOtvVBD
    {
        public int IDSpec;
        public List<ZadOtvVBD> ZadOtvVBD { get; set; }
        private SotaContext db;

        public StaticZadOtvVBD(SotaContext db,int IdSpec)
        {
            this.db = db;
            IDSpec = IdSpec;

        }

        public void Add()
        {
            ZadOtvVBD = db.Zadanie.Where(x=>x.IdSpec== IDSpec).Select(x => new ZadOtvVBD { ID = x.Id,Ball=Convert.ToDouble(db.StructSpec.FirstOrDefault(z=>z.IdSpec== IDSpec &&z.Number==x.Nomer && z.Type==1).Text), PriceError = x.PriceError, Otvets = db.Otvet.Where(y => y.IdZadan == x.Id && y.Ustar==0 && y.Verno==1).Select(y => new OtvetVBDs { ID=y.Id,Text = y.Text.Replace(" ","").ToUpper(),Param=y.Param1, Ball = y.Ball }).ToList() }).ToList();
           

        }
       

 }

    public class ZadOtvVBD
    {
        public int ID { get; set; }
        public double Ball { get; set; }
        public double PriceError { get; set; }
        public List<OtvetVBDs> Otvets { get; set; }
    }
    public class OtvetVBDs
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public double Param { get; set; }
        public double Ball { get; set; }
    }
}
    