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
        public string Tema { get; set; }

    }
    public class ResultTest
    {
        public List<BallPoZadan> Result;
        private int _idUser;
        private int _idRabota;
        private SotaContext db;

        public ResultTest(int idUser, int idRabota, SotaContext context)
        {
            _idUser = idUser;
            _idRabota = idRabota;
            db = context;
            GetResult();
        }

        public void GetResult()
        {
            Result = db.UsersBalls.Where(x => x.IdUser == _idUser && x.IdRabota == _idRabota).Join(db.Zadanie,
                x => x.IdZadania, y => y.Id, (x, y) => new BallPoZadan { Number = y.Nomer, Ball = x.Ball, Tema = y.Tema }).OrderBy(x => x.Number).ToList();

        }

    }
}
