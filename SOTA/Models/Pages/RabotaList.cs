using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models
{

    public class RabotaTabl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdSpec { get; set; }
        public int Dliteln { get; set; }
        public string UrovenRabot { get; set; }
        public DateTime Nachalo { get; set; }
        public DateTime Konec { get; set; }
        public string ListUchasn { get; set; }
        public DateTime Sozd { get; set; }
        // public int SpecId { get; set; }
        public string SpecN { get; set; }
        public string TipN { get; set; }
        public string PredmN { get; set; }
        public int KlassR { get; set; }

    }

    public class RabotaTablList
    {
        public List<RabotaTabl> RabotaTabls { get; set; }
    }

    public class RabotaList
    {
        public Rabota Rabot { get; set; }
        public List<Rabota> Rabotas { get; set; }
        public List<Specific> Specs { get; set; }
        public List<Predm> Predms { get; set; }
        public List<Oo> Oos { get; set; }
        public List<Mo> Mos { get; set; }
        public List<SChecNaznachMo> NaznachMos { get; set; }
        // public List<SChecNaznachOo> NaznachOos { get; set; }
    }
    public class SChecNaznachOo
    {
        public Oo OO { get; set; }
        public bool Chec { get; set; } = false;


    }
    public class SChecNaznachMo
    {
        public Mo MO { get; set; }
        public bool Chec { get; set; } = false;
        public List<SChecNaznachOo> OO { get; set; }

        //public SChecNaznachMo(SotaContext db,int id, bool check)
        //{

        //}



    }
    public class ListSchecMO
    {
        List<SChecNaznachMo> sChecsMO { get; set; }
        SotaContext db;


        public List<SChecNaznachMo> SChecNaznachMo(SotaContext _db, int id)
        {


            db = _db;

            sChecsMO = new List<SChecNaznachMo>();
            SChec(SpisokMOCheck(id), id);
            return sChecsMO;


        }
        private List<NaznachMo> SpisokMOCheck(int id)
        {
            List<NaznachMo> NaznachMo = db.NaznachMo.Where(x => x.IdRab == id).ToList();
            return NaznachMo;


        }
        private void SChec(List<NaznachMo> NaznachMo, int idRabota)
        {
            List<Mo> MO = db.Mo.ToList();
            foreach (Mo row in MO)
            {
                SChecNaznachMo Znach = new SChecNaznachMo();
                Znach.MO = row;

                Znach.Chec = NaznachMo.Where(x => x.IdMo == row.Id).Any() ? true : false;
                List<SChecNaznachOo> sChecs = new List<SChecNaznachOo>();
                ListSchecOO ListOO = new ListSchecOO();
                Znach.OO = ListOO.SChecNaznachOo(db, idRabota, Znach.Chec, row.Id).ToList();

                sChecsMO.Add(Znach);

            }
        }



    }


    public class ListSchecOO
    {
        List<SChecNaznachOo> sChecs { get; set; }
        SotaContext db;
        public List<SChecNaznachOo> SChecNaznachOo(SotaContext _db, int id, bool checkMO, int _idMO)
        {


            db = _db;
            if (checkMO)
            {
                sChecs = new List<SChecNaznachOo>();
                SChec(_idMO);
            }
            else
            {
                sChecs = new List<SChecNaznachOo>();
                SChec(SpisokOOCheck(id), _idMO);
            }
            return sChecs;


        }
        private List<NaznachOo> SpisokOOCheck(int id)
        {
            List<NaznachOo> NaznachOo = db.NaznachOo.Where(x => x.IdRab == id).ToList();
            return NaznachOo;


        }
        private void SChec(int _idMO)
        {
            List<Oo> OO = db.Oo.Where(x => x.IdMo == _idMO).ToList();
            foreach (Oo row in OO)
            {
                SChecNaznachOo Znach = new SChecNaznachOo();
                Znach.OO = row;
                sChecs.Add(Znach);
            }
        }
        private void SChec(List<NaznachOo> NaznachOo, int _idMO)
        {
            List<Oo> OO = db.Oo.Where(x => x.IdMo == _idMO).ToList();
            foreach (Oo row in OO)
            {
                SChecNaznachOo Znach = new SChecNaznachOo();
                Znach.OO = row;

                Znach.Chec = NaznachOo.Where(x => x.IdOo == row.Id).Any() ? true : false;
                sChecs.Add(Znach);

            }
        }
    }



}
