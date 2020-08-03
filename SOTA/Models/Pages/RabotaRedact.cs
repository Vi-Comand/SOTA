using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{
    public class Spec
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Predm { get; set; }
        public string Tip { get; set; }


    }

    public class RabotaRedact
    {
        public Rabota Rabot { get; set; }

        public List<Spec> Specicfikac;
        public List<Oo> Oos { get; set; }
        public List<Mo> Mos { get; set; }
        public List<SChecNaznachMo> NaznachMos { get; set; }

        public RabotaRedact(SotaContext context, int id)
        {
            Rabot = context.Rabota.Find(id);
            Specicfikac = (from Spec in context.Specific
                join p in context.Predm on Spec.Predm equals p.Id into gj
                from Predm in gj.DefaultIfEmpty()

                join y in context.TipSpec on Spec.Tip equals y.Id into gj1
                from Tipspec in gj1.DefaultIfEmpty()

                select new Spec
                {
                    Id = Spec.Id,
                    Name = Spec.Name,
                    Predm = Predm.Name,
                    Tip = Tipspec.Name
                }).ToList();
            Oos = context.Oo.ToList();
            Mos = context.Mo.ToList();
            ListSchecMO ListMO = new ListSchecMO();
            NaznachMos = ListMO.SChecNaznachMo(context, Rabot.Id);
        }


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
