using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models.Pages.Reports
{

    public class ReportTip1
    {
        SotaContext db;
     
        int idRabota;
    
        
        ProtokolProvedeniaMonitoringovoyRaboti protokol;
        public ReportTip1(SotaContext context,int idRabota)
        {
            this.idRabota = idRabota;
            db = context;
        }
        public void Create()
        {
            protokol = new ProtokolProvedeniaMonitoringovoyRaboti();
            FillingProtocol();
            BuildExcelReports Excel = new BuildExcelReports(protokol);
            Excel.CreateFile();

        }
        private void ListForTables()
        {

      

        }
       private void FillingProtocol()
        {
            Rabota rab = db.Rabota.Find(idRabota);
            protokol.NameR = rab.Name;
            protokol.DateProved = rab.Nachalo.Date;
            protokol.Predmet = db.Predm.Find(db.Specific.Find(rab.IdSpec).Predm).Name;          
            protokol.KolVar = db.Zadanie.Where(x => x.IdSpec == rab.IdSpec).OrderByDescending(x => x.Variant).First().Variant;
            GeneretionListBalls listBalls = new GeneretionListBalls(db);
            GeneretionListUchen list = new GeneretionListUchen(listBalls.Get(), db);
            protokol.Tables = list.GetTables();
            protokol.ProcVipZad = list.GetProcVip();
            protokol.KolUch = protokol.Tables.Count;
        }

    }


}




