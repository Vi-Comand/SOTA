using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{
    interface IRaschetBallsUser
    {
    }

    public class Test
    {
        public List<ZadanNaProsmotr> Zadans { get; }
        private readonly int _idSpec;
        private readonly int _nVar;
        private readonly SotaContext _db;

    }
    public class OtvTest
    {
        public int _id { get; set; }
        public string _text { get; set; }
        public int _verno { get; set; }

    }
    public class ZadanTest
    {
        public int _idZadan { get; }
        public int _tip { get; set; }
        public string _text { get; }
        public string _Doptext { get; }
        public string _otvVBD { get; set; }
        public int _nomer { get; }
        private int _idRabota;
        private int _idUser;
        private Zadanie Zadan;
        public List<OtvTest> _Otv { get; set; }

        private SotaContext _db;
        public ZadanTest(int id, SotaContext db, int idRabota, int idUser)
        {
            _idZadan = id;
            if (id != 0 && db != null)
            {
                _idUser = idUser;
                Zadan = db.Zadanie.Find(id);
                _tip = Zadan.Tip;
                _text = Zadan.Text;
                _Doptext = Zadan.Doptext;
                _db = db;
                _idRabota = idRabota;
                _nomer = Zadan.Nomer;

            }
            if (Zadan.Tip == 2)
                LoadOtvetTip2();
            if (Zadan.Tip == 4)
                LoadOtvetTip4();
            LoadOtvVBD();

        }
        private void LoadOtvVBD()
        {
            try
            {
                _otvVBD = _db.AnswerUser.Where(x => x.IdZadan == _idZadan && x.IdRabota == _idRabota && x.IdUser == _idUser).First().TextOtv;
            }
            catch { }

        }
        private void LoadOtvetTip4()
        {

            _Otv = _db.Otvet.Where(x => x.IdZadan == _idZadan && Math.Round(x.Param1 - Math.Truncate(x.Param1), 1) == 0.1 && x.Ustar == 0).Select((q) => new OtvTest
            {
                _id = q.Id,
                _text = q.Text,


            }).ToList();

        }

        private void LoadOtvetTip2()
        {
            _Otv = _db.Otvet.Where(x => x.IdZadan == _idZadan && x.Ustar == 0).Select((q) => new OtvTest
            {
                _id = q.Id,
                _text = q.Text

            }).ToList();
            int kol_verno = _db.Otvet.Where(x => x.IdZadan == _idZadan && x.Ustar == 0 && x.Verno == 1).Count();
            if (kol_verno == 1)
                _tip = 3;
        }


    }
    public class VarintTest
    {
        public int _idRabota { get; }
        public int _variant { get; }
        public List<ZadanTest> _Zadan { get; }
        private int[] _idZadans;
        private int _idUser;
        private SotaContext _db;
        int _idSpec;
        public VarintTest(int idRabota, int variant, SotaContext db, int idUser)
        {
            _idRabota = idRabota;
            _variant = variant;
            _idUser = idUser;
            _Zadan = new List<ZadanTest>();
            _db = db;
            if (_db != null)
                _idSpec = _db.Rabota.Where(x => x.Id == idRabota).First().IdSpec;
            GetListZadan();
        }
        private void SpisokIdZadan()
        {
            _idZadans = _db.Zadanie.Where(x => x.IdSpec == _idSpec && x.Variant == _variant).Select(g => g.Id).ToArray();

        }
        private void GetListZadan()
        {
            SpisokIdZadan();
            for (int i = 0; i < _idZadans.Length; i++)
            {
                ZadanTest Zadan = new ZadanTest(_idZadans[i], _db, _idRabota, _idUser);
                _Zadan.Add(Zadan);
            }

        }


    }
    public class SaveOtvUser
    {
        AnswerUser answer = new AnswerUser();

        SotaContext db;
        public SaveOtvUser(int _id, string _text, SotaContext _db, int _idUser, int _idRabota, int proveren)       {
            if (_id != 0 && _text != null && _db != null && _idUser != 0 && _idRabota != 0)
            {
                db = _db;
                if (db.AnswerUser.FirstOrDefault(x => x.IdZadan == _id && x.IdRabota == _idRabota && x.IdUser == _idUser) != null)
                {
                    answer = db.AnswerUser.First(x => x.IdZadan == _id && x.IdRabota == _idRabota && x.IdUser == _idUser);

                    answer.Proveren = proveren;
                    answer.TextOtv = _text;
                    SaveChange();
                }
                else
                {
                    answer.IdZadan = _id;
                    answer.Proveren = proveren;
                    answer.TextOtv = _text;
                    answer.IdRabota = _idRabota;
                    answer.IdUser = _idUser;

                    SaveVBD();
                }






            }

        }

        private void SaveChange()
        {
            answer.Date = DateTime.Now;

            db.SaveChanges();
        }
        private void SaveVBD()
        {

            answer.Date = DateTime.Now;
            db.AnswerUser.Add(answer);
            db.SaveChanges();

        }

    }

    public class OpredelenieVariant
    {
        private int idRabota;
        private int Variant;
        private int idUser;
        private SotaContext db;

        public OpredelenieVariant(int _idRabota, int _idUser, SotaContext _db)
        {
            if (_idRabota != 0)
            {
                idRabota = _idRabota;
                Variant = 0;
                idUser = _idUser;
                db = _db;
            }
        }

        private void VBD()
        {
            Variant = db.VariantUser.Where(x => x.IdRabota == idRabota && x.IdUser == idUser).Select(x => x.Variant).FirstOrDefault();
        }
        public int GetVariant()
        {
            VBD();
            if (Variant == 0)
            {

                PrisvoitVariant();

            }

            return Variant;
        }

        private void PrisvoitVariant()
        {
            int IdSpec = db.Rabota.Find(idRabota).IdSpec;

            int KolVar = db.Zadanie.Where(x => x.IdSpec == IdSpec).OrderByDescending(x => x.Variant).First().Variant;
            if (KolVar != 0)
            {

                Random rnd = new Random();

                //Получить случайное число (в диапазоне от 0 до 10)
                Variant = rnd.Next(1, KolVar + 1);



                ZapisVBD();
            }
        }

        private void ZapisVBD()
        {
            VariantUser newStr = new VariantUser
            {
                Variant = Variant,
                IdRabota = idRabota,
                IdUser = idUser,
                Date = DateTime.Now
            };
            db.Add(newStr);
            db.SaveChanges();
        }
    }


}
