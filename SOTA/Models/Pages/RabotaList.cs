﻿using System;
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

    public class ReportsRabotaList
    {public int idOO { get; }
        public List<RabotaUchen> RabotaTabls { get; set; }
    }


        public class RabotaUchenList
    {
        public List<RabotaUchen> RabotaTabls { get; set; }
    }

    public class FormirRabotaTablList
    {

      //  Users User;
          SotaContext _db;
        DateTime dateNow;
        int _idOO;
        int _idMO;
        int klass;
        public FormirRabotaTablList(Users user, SotaContext db)
        {
            _db = db;
          
            _idOO=user.IdOo;
          _idMO = user.IdMo;
            dateNow = DateTime.Now;
            klass=db.Klass.Find(user.IdKlass).KlassNom;
         
        }
        public FormirRabotaTablList(int idOO, SotaContext db)
        {
            _db = db;
            _idOO = idOO;
            _idMO = db.Oo.Where(x => x.Id == idOO).First().IdMo;
            dateNow = DateTime.Now;
            

        }

        public  RabotaUchenList GetSpisokRabotUchen()
        {
            RabotaUchenList rabotaList = new RabotaUchenList();
            _db.Rabota.Where(x => x.Klass == klass && x.Konec > dateNow).ToList();
            List<Rabota> list = GetSpisokRabot(_db.Rabota.Where(x => x.Klass == klass && x.Konec > dateNow).ToList());
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

        public ReportsRabotaList GetSpisokRabotReports()
        {
            ReportsRabotaList rabotaList = new ReportsRabotaList();
            _db.Rabota.Where(x => x.Klass == klass && x.Konec < dateNow).ToList();
            List<Rabota> list = GetSpisokRabot(_db.Rabota.Where(x =>x.Konec < dateNow).ToList());
            if (list.Count != 0)
                rabotaList.RabotaTabls = (from rab in list

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
                                              KlassR = rab.Klass,
                                              Nachalo = rab.Nachalo,
                                              Konec = rab.Konec,
                                              PredmN = Predm.Name,
                                              TipN = Tip.Name
                                          }).ToList();

            return rabotaList;
        }
        private List<Rabota> GetSpisokRabot(List<Rabota> list)
        {
           


            


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
            var spisok = from Naznach in _db.NaznachMo.Where(x => x.IdMo == _idMO)

                         join Rab in rabot on Naznach.IdRab equals Rab.Id 
                        
                         select Rab;

            return spisok.ToList();
        }
        private List<Rabota> GetSpisokRabotOO(List<Rabota> rabot)
        {

            var spisok = from Naznach in _db.NaznachOo.Where(x => x.IdOo == _idOO)

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




