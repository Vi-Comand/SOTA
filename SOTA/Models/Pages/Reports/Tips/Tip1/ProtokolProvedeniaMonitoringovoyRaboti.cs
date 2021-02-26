using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Reports
{
    public class ProtokolProvedeniaMonitoringovoyRaboti
    {
        public string NameR { get; set; }
        public string Predmet { get; set; }
        public DateTime DateProved { get; set; }
        public int KolVar { get; set; }
        public int KolZad { get; set; }
        public int IdSpec { get; set; }
        public int KolUch { get; set; }
        public List<RowProtokol> Tables { get; set; }
        public List<double> ProcVipZad { get; set; }
        public List<double> SumVipZad { get; set; }
        public List <RowAnalytics> AnalyticsTable { get; set; }

    }

       public class RowProtokol
    {
        public int Id { get; set; }
        public string MO { get; set; }
        public string OO { get; set; }
        public string FIO { get; set; }
        public string Klass { get; set; }
        public int Var { get; set; }
        public List<double> Balls { get; set; }
        public Double ProcVipUch { get; set; }
        public Double SumBall { get; set; }
        
        

    }
    public class RowAnalytics
    {
        public int Number { get; set; }
        //Проверяемы элемент содержания
        public string CheckElementContent { get; set; }
        //Код УУД или Коды проверяемых требований
        public string KodPt { get; set; }
        //Коды проверяемых элементов содержания
        public string KodPes { get; set; }
        //Уровень сложности
        public string LevelOfComplexity { get; set; }
        //Максимальный балл
        public double MaxScore { get; set; }
        //Средний балл
        public double AverageScore { get; set; }
        //Уровень успешности
        public double LevelSuccess { get; set; }
        //Заключение по заданиям
        public string Conclusion { get; set; }
        //Рекомендации 
        public string Recomend { get; set; }
    }


}
