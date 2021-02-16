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
            var str = db.StructSpec.Where(x => x.IdSpec == SpecID).ToList();
            //Result = (from Ub in db.UsersBalls.Where(x => x.IdUser == _idUser && x.IdRabota == _idRabota)
            //          join Zd in db.Zadanie on Ub.IdZadania equals Zd.Id into zd
            //          from Zd in zd.DefaultIfEmpty()
            //          select new BallPoZadan
            //          {
            //              Number = Zd.Nomer,
            //              Ball = Ub.Ball,
            //              MaxBall = str.Where(x => x.Type == 1 && x.Number == Zd.Nomer) != null ? Convert.ToDouble(str.Where(x => x.Type == 1 && x.Number == Zd.Nomer).First().Text) : 0,
            //              Tema = str.Where(x => x.Type == 2 && x.Number == Zd.Nomer) != null ? str.Where(x => x.Type == 2 && x.Number == Zd.Nomer).First().Text : "",
            //              Urov = str.Where(x => x.Type == 3 && x.Number == Zd.Nomer) != null ? str.Where(x => x.Type == 3 && x.Number == Zd.Nomer).First().Text : "",
            //              RekomU = str.Where(x => x.Type == 6 && x.Number == Zd.Nomer) != null ? str.Where(x => x.Type == 6 && x.Number == Zd.Nomer).First().Text : ""
            //          }).OrderBy(x => x.Number).ToList();
            
            AnalyticsTable = db.Zadanie.Where(x => x.IdSpec == SpecID && x.Variant==1).Select(y => new RowAnalytics
            {
                Number = y.Nomer,
                CheckElementContent = str.Where(x => x.Type == 2 && x.Number == y.Nomer).Any() ? str.Where(x => x.Type == 2 && x.Number == y.Nomer).First().Text : "",
                LevelOfComplexity = str.Where(x => x.Type == 3 && x.Number == y.Nomer).Any() ? str.Where(x => x.Type == 3 && x.Number == y.Nomer).First().Text : "",
                MaxScore = str.Where(x => x.Type == 1 && x.Number == y.Nomer).Any() ? Convert.ToDouble(str.Where(x => x.Type == 1 && x.Number == y.Nomer).First().Text) : 0,
                KodPt = str.Where(x => x.Type == 5 && x.Number == y.Nomer).Any() ? str.Where(x => x.Type == 5 && x.Number == y.Nomer).First().Text : "",
                KodPes = str.Where(x => x.Type == 4 && x.Number == y.Nomer).Any() ? str.Where(x => x.Type == 4 && x.Number == y.Nomer).First().Text : "",
                Recomend = str.Where(x => x.Type == 7 && x.Number == y.Nomer).Any() ? str.Where(x => x.Type == 7 && x.Number == y.Nomer).First().Text : ""
            }).OrderBy(x=>x.Number).ToList();
       
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
            if (LevelSuccess < 0.30)
                return "Данный элемент содержания усвоен на крайне низком уровне.Требуется серьёзная коррекция.";
            if (LevelSuccess < 0.50)
                return "Данный элемент содержания усвоен на низком уровне. Требуется коррекция.";
            if (LevelSuccess < 0.70)
                return "Данный элемент содержания усвоен на приемлемом уровне. Возможно, необходимо обратить внимание на категорию учащихся, затрудняющихся с данным заданием.";
            if (LevelSuccess < 0.90)
                return "Данный элемент содержания усвоен на хорошем уровне. Важно поддерживать этот уровень у сильных учащихся и продолжать подготовку слабых учащихся";

            return "Данный элемент содержания усвоен на высоком уровне. Важно зафиксировать данный уровень. Обратить внимание на причины и условия обеспечившие высокий результат.";
        }
    }
}
