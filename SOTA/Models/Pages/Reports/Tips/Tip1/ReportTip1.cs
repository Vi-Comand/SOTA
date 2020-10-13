﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models.Pages.Reports
{

    public class ReportTip1:ReportGenerator
    {
        SotaContext db;
     
        int idRabota;
        int idOO;
        int idMO;
        ProtokolProvedeniaMonitoringovoyRaboti protokol;
        public ReportTip1( SotaContext context, int idRabota, Oo OO)
        {
            db = context;
            this.idRabota = idRabota;
           
            this.idOO = OO.Id;
        }
        public ReportTip1( SotaContext context, int idRabota, Mo MO)
        {
            db = context;
            this.idRabota = idRabota;
        
            this.idMO = MO.Id;
        }
        public override string Create()
        {
            protokol = new ProtokolProvedeniaMonitoringovoyRaboti();
            FillingProtocol();
            BuildExcelReports Excel = new BuildExcelReports(protokol);
            Excel.CreateFile();
            return Excel.GetPath();

        }
       
       private void FillingProtocol()
        {
            Rabota rab = db.Rabota.Find(idRabota);
            protokol.NameR = rab.Name;
            protokol.DateProved = rab.Nachalo.Date;
            protokol.Predmet = db.Predm.Find(db.Specific.Find(rab.IdSpec).Predm).Name;          
            protokol.KolVar = db.Zadanie.Where(x => x.IdSpec == rab.IdSpec).OrderByDescending(x => x.Variant).First().Variant;
           
            GeneretionListUchen list = new GeneretionListUchen(FillingBall(), db);
            protokol.Tables=list.GetTables();
            protokol.ProcVipZad = list.GetProcVip();
            protokol.KolUch = protokol.Tables.Count;
        }
        private List<RowForTable> FillingBall()
        {
            List<RowForTable> list = new List<RowForTable>();
            GeneretionListBalls listBalls = new GeneretionListBalls(db, idRabota);
            if (idOO != 0)
            {
                list.AddRange(listBalls.Get(idOO));
            }
            if (idMO != 0)
            {
                foreach( var row in ListMO())

                list.AddRange(listBalls.Get(row));
            }
            return list;
        }
        private List<int> ListMO()
        {
            List<int> list = new List<int>();
            list = db.Oo.Where(x => x.IdMo == idMO).Select(x => x.Id).ToList();
            return list;
        }

    }


}



