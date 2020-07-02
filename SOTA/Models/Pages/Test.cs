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
        public int _verno { get; set; }

    }
    public class ZadanTest 
    {
       public  int _idZadan { get; }
       public  int _tip { get; set; }
       public string _text { get;  }
        public string _otvVBD { get; set; }
        public int _nomer { get; }

        private Zadanie Zadan;
       public List<OtvTest> _Otv { get; set; }

        private SotaContext _db;
        public ZadanTest(int id,SotaContext db)
        {
            _idZadan = id;
            if (id != 0 && db!=null) 
            {
                Zadan = db.Zadanie.Find(id);
                _tip = Zadan.Tip;
                _text = Zadan.Text;
                _db = db;
                
                _nomer = Zadan.Nomer;
             
            }
            if(Zadan.Tip==2)
            LoadOtvetTip2();
            if(Zadan.Tip==4)
                LoadOtvetTip4();
            LoadOtvVBD();

        }
        private void LoadOtvVBD()
        {
            try
            {
                _otvVBD = _db.AnswerUser.Where(x => x.IdZadan == _idZadan).First().TextOtv;
            }
            catch { }
                      
           }
            private void LoadOtvetTip4()
        {
            _Otv = _db.Otvet.Where(x => x.IdZadan == _idZadan && x.Param1<2 && x.Ustar == 0).Select((q) => new OtvTest
            {
                _id = q.Id,
                _text = q.Text,
               

            }).ToList();

        }

        private void LoadOtvetTip2()
        {
            _Otv = _db.Otvet.Where(x => x.IdZadan == _idZadan && x.Ustar == 0).Select((q) => new OtvTest
            {
                _id = q.Id, _text = q.Text

            }).ToList() ;
            int kol_verno = _db.Otvet.Where(x => x.IdZadan == _idZadan && x.Ustar == 0 && x.Verno == 1).Count();
            if (kol_verno == 1)
                _tip = 3;
        }


    }
    public class VarintTest
    {
      public int _idRabota { get;  }
       public  int _variant { get;  }
        public List<ZadanTest> _Zadan { get;  }
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
            GetListZadan();
        }
        private void SpisokIdZadan()
        {
            _idZadans = _db.Zadanie.Where(x => x.IdSpec == _idSpec&& x.Variant==_variant).Select(g => g.Id).ToArray();

        }
        private void GetListZadan()     
        {
            SpisokIdZadan();
            for (int i = 0; i < _idZadans.Length; i++)
            {
                ZadanTest Zadan = new ZadanTest(_idZadans[i],_db);
                _Zadan.Add(Zadan);
            }
            
        }
      

    }
    public class SaveOtvUser
    {
        AnswerUser answer;
      
        SotaContext db;
        public SaveOtvUser(int _id,string _text,SotaContext _db,int _idUser)
        {
            if(_id!=0 && _text!= null&& _db!=null && _idUser!=0)
            {
                answer = new AnswerUser();
                answer.IdZadan = _id;
                answer.TextOtv = _text;
                answer.IdUser = _idUser;
           
                db = _db;
                SaveVBD();


            }

        }
        public void SaveVBD()
        {
          
            answer.Date = DateTime.Now;
            db.AnswerUser.Add(answer);
            db.SaveChanges();

        }

    }

}
