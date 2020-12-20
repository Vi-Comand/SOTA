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
        ProtokolProvedeniaMonitoringovoyRaboti Protokol;
        List<RowAnalytics> AnalyticsTable=new List<RowAnalytics>();
        public CreatingAnalyticsTable(SotaContext db,int RabotaID,ProtokolProvedeniaMonitoringovoyRaboti Protokol)
        {
            this.db = db;
            this.RabotaID = RabotaID;
            this.Protokol = Protokol;
        }
        public List<RowAnalytics> Get()
        {
            if (Protokol != null)
            {
                FillingAnalyticsTableFromBD();
                CalculationAnalyticsTable();
            }
            return AnalyticsTable;

        }
        private void FillingAnalyticsTableFromBD()
        {
            int SpecID = db.Rabota.Find(RabotaID).IdSpec;
            AnalyticsTable = db.Zadanie.Where(x => x.IdSpec == SpecID && x.Variant==1).Select(y => new RowAnalytics { Number = y.Nomer, CheckElementContent = y.Tema, LevelOfComplexity = y.Urov, MaxScore=y.Ball==0?db.Otvet.Where(x=>x.IdZadan==y.Id&&x.Ustar==0).Select(b=>b.Ball).Sum(): y.Ball }).OrderBy(x=>x.Number).ToList();
       
        }
        private void CalculationAnalyticsTable()
        {

            double[][] ArrayScore = GetArrayScore();
          for (int i=0;i< AnalyticsTable.Count; i++)
            {
                AnalyticsTable[i].LevelSuccess = Protokol.ProcVipZad[i];
                AnalyticsTable[i].AverageScore= CalculationAverageScore(ArrayScore[i].ToList());
                AnalyticsTable[i].Conclusion = GetConclusion(AnalyticsTable[i].LevelSuccess);

            }
        }


        private double[][] GetArrayScore()
        {
            int CountTasks = Protokol.Tables[0].Balls.Count;
            int CountRows = Protokol.Tables.Count;
            double[][] Array = new double[CountTasks][];
            int j = 0;
            foreach (var row in Protokol.Tables)
            {

                for(int i=0;i<CountTasks;i++)
                {
                    if (j == 0)
                    {
                        Array[i] = new double[CountRows];
                    }

                    Array[i][j] = row.Balls[i];
                }
                j++;
            }
            return Array;

        }

        private double CalculationAverageScore(List<double> Scores)
        {
            double TotalScore = 0;
            foreach(double score in Scores)
            { double point = 0;
                if (score != -1)
                    point = score;
                TotalScore += point;

            }

            return TotalScore / Scores.Count;
           

        }
        private string GetConclusion(double LevelSuccess)
        {
            if (LevelSuccess < 30)
                return "Данный элемент содержания усвоен на крайне низком уровне.Требуется серьёзная коррекция.";
            if (LevelSuccess < 50)
                return "Данный элемент содержания усвоен на низком уровне. Требуется коррекция.";
            if (LevelSuccess < 70)
                return "Данный элемент содержания усвоен на приемлемом уровне. Возможно, необходимо обратить внимание на категорию учащихся, затрудняющихся с данным заданием.";
            if (LevelSuccess < 90)
                return "Данный элемент содержания усвоен на хорошем уровне. Важно поддерживать этот уровень у сильных учащихся и продолжать подготовку слабых учащихся";

            return "Данный элемент содержания усвоен на высоком уровне. Важно зафиксировать данный уровень. Обратить внимание на причины и условия обеспечившие высокий результат.";
        }
    }
}
