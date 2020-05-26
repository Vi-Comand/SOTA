using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{

    public class Variants
    {
        public int IdRabota { get;  }

        public int IdSpec { get; }

        public int KolVar { get; }
        

        private SotaContext _db;
        public Variants(int idRabota,SotaContext db)
        {
            IdRabota = idRabota;
            _db = db;

            if (_db != null)
            {
                IdSpec = _db.Rabota.Find(IdRabota).IdSpec;

                KolVar = _db.Zadanie.Any(x => x.IdSpec == IdSpec)
                    ? _db.Zadanie.Where(x => x.IdSpec == IdSpec).OrderByDescending(x => x.Variant).First().Variant
                    : 0;
            }
        }

    }

}
