using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models
{

    public class ListKriter
    {
        public int Id { get; set; }
        public int IdZad { get; set; }
        public string Text { get; set; }
        public int Ball { get; set; }
    }


        public class ListRespons
    {
        public int IdOtv { get; set; }
        public int IdZad { get; set; }
        public int IdRab { get; set; }
        public string TextZad { get; set; }
        public int NomZad { get; set; }
        public string OtvetUzer { get; set; }
        public List<ListKriter> ListKriters { get; }
        public DateTime DOtv { get; set; }
        public DateTime DRabKonec { get; set; }
        public string Predm { get; set; }
        public string NazvRab { get; set; }
        public int KlassR { get; set; }
    }


    public class SelectRespons
    {
        public List<ListRespons> ListResp { get; }
       /* private readonly int _idSpec;
        private readonly int _nVar;
        private readonly int _idRab;*/
        private readonly SotaContext _db;
        public SelectRespons(SotaContext context)
        {

            _db = context;
          

            ListResp = (from AnswerUser in _db.AnswerUser
                           join z in _db.Zadanie on AnswerUser.IdZadan equals z.Id into gj
                           from Zadan in gj.DefaultIfEmpty()

                           join r in _db.Rabota on AnswerUser.IdRabota equals r.Id into gj1
                           from Rabot in gj1.DefaultIfEmpty()

                           select new ListRespons
                           {
                               IdOtv = AnswerUser.Id,
                               IdRab = Rabot.Id,
                               IdZad = Zadan.Id,
                               TextZad = Zadan.Text
                           }).ToList();
        }

    }

}
