using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// TODO: Скрывать прошедшие спецификации поле в базе hiden, в таблиу добавить chekbox 
namespace SOTA.Models
{
    public class SchetchikZadVSpecs
    {
        List<KolZadVSpec> List;
        public List<KolZadVSpec> Poschitat(List<int> ListIdSpec, SotaContext db)
        {
            List = new List<KolZadVSpec>();

            List<Zadanie> ListZadan = db.Zadanie.ToList();
            foreach (int id in ListIdSpec)
            {
                KolZadVSpec str = new KolZadVSpec();
                int KolZadans = !db.Zadanie.Any(x => x.Variant == 1 && x.IdSpec == id) ? 0 : db.Zadanie.Count(x => x.Variant == 1 && x.IdSpec == id);
                str.Id = id;
                str.kolZad = KolZadans;
                List.Add(str);
            }
            return List;
        }

    }
    public class KolZadVSpec
    {
        public int Id { get; set; }
        public int kolZad { get; set; }
    }
    public class SpecifikacsList
    {
        public List<KolZadVSpec> KolZadVSpec { get; set; }
        public List<Specific> Spec { get; set; }
        public List<Predm> Predms { get; set; }
        public List<TipSpec> TipSpecs { get; set; }
    }



}
