using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models.Pages.Reports
{ 

    public class ReportGenerator
    {
        int _tip;
        SotaContext db;
        int idRab;
        public ReportGenerator(int tip, SotaContext context, int idRabota)
        {
            db = context;
            idRab = idRabota;
            _tip = tip;
        }
        
        public void Generaition()
        {
            if(_tip==1)
            {
                ReportTip1 Report = new ReportTip1(db,idRab);
                Report.Create();
            }
            


        }
    }


}




