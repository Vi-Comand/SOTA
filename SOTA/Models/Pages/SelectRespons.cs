using System;
using System.Collections.Generic;
using System.Linq;


namespace SOTA.Models
{


    public class ListRab
    {
        public List<Rabota> ListRabs { get; set; }
        public List<ListZad> ListZads { get; set; }
        public List<ListKriter> listKriters { get; set; }
    }

    public class Rab
    {
        public int IdRab { get; set; }
        public DateTime DRabNach { get; set; }
        public DateTime DRabKonec { get; set; }
        public string Predm { get; set; }
        public string NazvRab { get; set; }
        public int KlassR { get; set; }
    }



    public class ListZad
    {
        public int IdZad { get; set; }
        public int Var { get; set; }
        public int NomZad { get; set; }
    }

    public class ListKriter
    {
        public int Id { get; set; }
        public int IdZad { get; set; }
        public string Text { get; set; }
        public double Ball { get; set; }
    }

    public class SelectRespons
    {
        public List<Rabota> ListRabs { get; }
        public List<ListZad> ListZads { get; set; }
        public List<ListKriter> listKriters { get; set; }
        private readonly SotaContext _db;
        public SelectRespons(SotaContext context)
        {
            _db = context;

            var zadanies = _db.Zadanie.Where(x => x.Tip == 5).Select(x => x.IdSpec).ToArray();
            zadanies.Distinct();
            ListRabs = _db.Rabota.Where(r => r.Konec <= DateTime.Now).ToList();
            ListRabs = ListRabs.Where(x => zadanies.Contains(x.IdSpec)).ToList();

        }

        public List<Rabota> LRabots()
        {
            return (ListRabs);
        }
        public List<ListZad> LZadans(int idSpec)
        {
            ListZads = _db.Zadanie.Where(x => x.IdSpec == idSpec && x.Tip == 5).Select(x => new ListZad { IdZad = x.Id, Var = x.Variant, NomZad = x.Nomer }).ToList();
            return (ListZads);
        }

        public List<ListKriter> LKriter(int idZad)
        {
            listKriters = _db.Otvet.Where(x => x.IdZadan == idZad).Select(x => new ListKriter { Id = x.Id, Ball = x.Ball, IdZad = x.IdZadan, Text = x.Text }).ToList();
            return (listKriters);
        }

    }




}
