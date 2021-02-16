using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Test
{
    public class BallPoZadan
    {
        public int Number { get; set; }
        public double Ball { get; set; }
        public double MaxBall { get; set; }
        public string Tema { get; set; }
        public string Urov { get; set; }
        public string RekomU { get; set; }
    }
    public class ResultTest
    {
        public List<BallPoZadan> Result;
        private int _idUser;
        private int _idRabota;
        private int _idSpec;
        private SotaContext db;

        public ResultTest(int idUser, int idRabota, int idSpec, SotaContext context)
        {
            _idUser = idUser;
            _idRabota = idRabota;
            _idSpec = idSpec;
            db = context;
            GetResult();
        }

        public void GetResult()
        {
            var str = db.StructSpec.Where(x=>x.IdSpec==_idSpec).ToList();
            Result = (from Zd in db.Zadanie.Where(x => x.IdSpec == _idSpec && x.Variant==1)
                      // from Ub in db.UsersBalls.Where(x => x.IdUser == _idUser && x.IdRabota == _idRabota)
                      join Ub in db.UsersBalls.Where(x => x.IdUser == _idUser) on Zd.Id equals Ub.IdZadania into ub
                      from Ub in ub.DefaultIfEmpty()
                      select new BallPoZadan
                      {
                          Number = Zd.Nomer,
                          Ball = Ub.Ball!=null?Ub.Ball:0,
                          MaxBall = str.Where(x=>x.Type==1  && x.Number==Zd.Nomer).Count() != 0 ? Convert.ToDouble(str.Where(x => x.Type == 1 && x.Number == Zd.Nomer).First().Text) : 0,
                          Tema = str.Where(x => x.Type == 2 && x.Number == Zd.Nomer).Count() != 0 ? str.Where(x => x.Type == 2 && x.Number == Zd.Nomer).First().Text : "",
                          Urov = str.Where(x => x.Type == 3 && x.Number == Zd.Nomer).Count() != 0 ? str.Where(x => x.Type == 3 && x.Number == Zd.Nomer).First().Text : "",
                          RekomU = str.Where(x => x.Type == 6 && x.Number == Zd.Nomer).Count() != 0 ? str.Where(x => x.Type == 6 && x.Number == Zd.Nomer).First().Text : ""
                      }).OrderBy(x => x.Number).ToList();

        }

    }
}
