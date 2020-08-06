using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models
{

    public class RabotaUchen
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

    public class RabotaUchenList
    {
        public List<RabotaUchen> RabotaTabls { get; set; }
    }

    public class FormirRabotaTablList
    {

        Users User;
          SotaContext _db;
        DateTime dateNow;
        int klass;
        public FormirRabotaTablList(Users user, SotaContext db)
        {
            _db = db;
            User = user;
            dateNow = DateTime.Now;
            klass=db.Klass.Find(User.IdKlass).KlassNom;
         
        }


       public  RabotaUchenList GetSpisokRabotUchen()
        {
            RabotaUchenList rabotaList = new RabotaUchenList();
            List<Rabota> list = GetSpisokRabot();
            if(list.Count!=0)
                        rabotaList.RabotaTabls =   (from rab in list

                                                    join SpecK in _db.Specific on rab.IdSpec equals SpecK.Id into spK
                                                    from SK in spK.DefaultIfEmpty()
                                                    join Pred in _db.Predm on SK.Predm equals Pred.Id into pr
                                                    from Predm in pr.DefaultIfEmpty()
                                                    join Tip in _db.TipSpec on SK.Tip equals Tip.Id into T
                                                    from Tip in T.DefaultIfEmpty()



                                                    select new RabotaUchen
                                                    {
                                                        Id = rab.Id,
                                                        Name = rab.Name,
                                                        Dliteln = rab.Dliteln,
                                                        Nachalo = rab.Nachalo,
                                                        Konec = rab.Konec,
                                                        PredmN = Predm.Name,
                                                        TipN = Tip.Name
                                                    }).ToList();

            return rabotaList;
        }


        private List<Rabota> GetSpisokRabot()
        {
           


            List<Rabota> list = _db.Rabota.Where(x => x.Klass == klass && x.Konec > dateNow).ToList();


            List<Rabota> ListRabot = new List<Rabota>();

            ListRabot.AddRange(GetSpisokRabotKray(list));
            ListRabot.AddRange(GetSpisokRabotMO(list));
            ListRabot.AddRange(GetSpisokRabotOO(list));


            return ListRabot;



        }
        private List<Rabota> GetSpisokRabotKray(List<Rabota> rabot)
        {
            var spisok = rabot.Where(x => x.UrovenRabot == "Край").ToList();
            return spisok;
        }
        private List<Rabota> GetSpisokRabotMO(List<Rabota> rabot)
        {
            var spisok = from Naznach in _db.NaznachMo.Where(x => x.IdMo == User.IdMo)

                         join Rab in rabot on Naznach.IdRab equals Rab.Id 
                        
                         select Rab;

            return spisok.ToList();
        }
        private List<Rabota> GetSpisokRabotOO(List<Rabota> rabot)
        {

            var spisok = from Naznach in _db.NaznachOo.Where(x => x.IdOo == User.IdOo)

                         join Rab in rabot on Naznach.IdRab equals Rab.Id
                        
                         select Rab;

            return spisok.ToList();
        }

    }

        public class RabotaList
    {
       //
        public List<Rabota> Rabotas { get; set; }
  
      
       
    }

   
    }




