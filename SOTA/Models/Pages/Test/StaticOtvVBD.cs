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
        static void Delete()
        {

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
            ZadOtvVBD = db.Zadanie.Where(x=>x.IdSpec== IDSpec).Select(x => new ZadOtvVBD { ID = x.Id, PriceError = x.PriceError, Otvets = db.Otvet.Where(y => y.IdZadan == x.Id).Select(y => new OtvetVBDs { Text = y.Text, Ball = y.Ball }).ToList() }).ToList();
           

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
        public string Text { get; set; }
        public double Ball { get; set; }
    }
}
    