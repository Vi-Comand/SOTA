using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Reports
{
    public class CreatingAnalyticsTable
    {
        SotaContext db;
        int RabotaID;
        List<RowAnalytics> AnalyticsTable=new List<RowAnalytics>();
        public CreatingAnalyticsTable(SotaContext db,int RabotaID)
        {
            this.db = db;
            this.RabotaID = RabotaID;
        }
        public List<RowAnalytics> Get(List<double> PercentageOfCorrectly)
        {
            FillingAnalyticsTableFromBD();
            CalculationAnalyticsTable(PercentageOfCorrectly);
            return AnalyticsTable;

        }
        private void FillingAnalyticsTableFromBD()
        {
            int SpecID = db.Rabota.Find(RabotaID).IdSpec;
            AnalyticsTable = db.Zadanie.Where(x => x.IdSpec == SpecID && x.Variant==1).Select(y => new RowAnalytics { Number = y.Nomer, CheckElementContent = y.Tema, LevelOfComplexity = y.Urov,MaxScore=y.Ball }).ToList();
        }
        private void CalculationAnalyticsTable(List<double> PercentageOfCorrectly)
        {
          for(int i=0;i< AnalyticsTable.Count; i++)
            {
                AnalyticsTable[i].

            }
        }
    }
}
