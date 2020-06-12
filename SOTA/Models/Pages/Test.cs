using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{

    public class Test
    {
        public List<ZadanNaProsmotr> Zadans { get; }
        private readonly int _idSpec;
        private readonly int _nVar;
        private readonly SotaContext _db;
     
    }
    public class OtvTest
    {
       public int _id{ get; set; }
        public string _text { get; set; }

    }
    public class ZadanTest 
    {
        int _idZadan { get; }
        int _tip { get;  }
        string _text { get;  }
        private Zadanie Zadan;
        List<OtvTest> _Otv { get; set; }
        private SotaContext _db;
        public ZadanTest(int id,SotaContext db)
        {
            _idZadan = id;
            if (id != 0 && db!=null) 
            {
                Zadan = db.Zadanie.Find(id);
                _tip = Zadan.Tip;
                _text = Zadan.Text;
                _Otv = LoadOtvet(id);
                _db = db;
            }
            LoadOtvet(_idZadan);
        }
        private List<OtvTest> LoadOtvet(int idZadan)
        {
            _Otv = _db.Otvet.Where(x => x.IdZadan == idZadan && x.Ustar == 0).Select((q) => new OtvTest { _id = q.Id, _text = q.Text }).ToList() ;
            return _Otv;
        }


    }
    public class VarintTest
    {
        int _idRabota { get;  }
        int _variant { get;  }
        List<ZadanTest> _Zadan { get; set; }
        private int[] _idZadans;
        private SotaContext _db;
        int _idSpec;
        public VarintTest(int idRabota,int variant,SotaContext db)
        {
            _idRabota = idRabota;
            _variant = variant;
            _Zadan = new List<ZadanTest>();
            _db = db;
            if(_db!=null)
            _idSpec = _db.Rabota.Where(x => x.Id == idRabota).First().IdSpec;
        }
        private void SpisokIdZadan()
        {
            _idZadans = _db.Zadanie.Where(x => x.IdSpec == _idSpec).Select(g => g.Id).ToArray();

        }
        public List<ZadanTest> GetListZadan()     
        {
            SpisokIdZadan();
            for (int i = 0; i < _idZadans.Length; i++)
            {
                ZadanTest Zadan = new ZadanTest(_idZadans[i],_db);
                _Zadan.Add(Zadan);
            }
            return _Zadan;
        }
      

    }


}
