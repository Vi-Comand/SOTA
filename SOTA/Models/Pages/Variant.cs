using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{

    public class Variant
    {
        public List<ZadanNaProsmotr> Zadans { get; }
        private readonly int _idSpec;
        private readonly int _nVar;
        private readonly SotaContext _db;
        public Variant(int idSpec, int nVar,SotaContext context)
        {
            this._idSpec = idSpec;
            this._nVar = nVar;
            _db = context;
            Zadans = new List<ZadanNaProsmotr>();
            ZapolnitZadans();
        }

        private protected void ZapolnitZadans()
        {
            List<Zadanie> SpisokVVar = _db.Zadanie.Where(x => x.IdSpec == _idSpec && x.Variant == _nVar).ToList();
            foreach (var row in SpisokVVar)
            {
                ZadanNaProsmotr formirZadan=new ZadanNaProsmotr(row,_db);
                Zadans.Add(formirZadan);
            }
           
  
        }

    }

    public class ZadanNaProsmotr
    {
        public Zadanie Zadan { get; }
        public List<Otvet> Otveti { get; set; }
        private readonly SotaContext _db;
        
        public ZadanNaProsmotr(Zadanie Zadan, SotaContext db)
        {
            this.Zadan = Zadan;
            _db = db;
            ZapolnitOtveti();
        }

        private void ZapolnitOtveti()
        {
          
            Otveti = _db.Otvet.Where(x => x.Ustar != 1 && x.IdZadan == Zadan.Id).ToList();
        }
    }

}
