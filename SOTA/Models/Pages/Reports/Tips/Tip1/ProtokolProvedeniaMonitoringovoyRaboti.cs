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
        public int KolUch { get; set; }
        public List<RowForTable> Tables { get; set; }
        public List<Double> ProcVipZad { get; set; }
    }

       public class RowForTable
    {
        public int Id { get; set; }
        public string MO { get; set; }
        public string OO { get; set; }
        public string FIO { get; set; }
        public string Klass { get; set; }
        public int Var { get; set; }
        public List<Double> Balls { get; set; }
        public Double ProcVipUch { get; set; }
        public Double SumBall { get; set; }
        
        

    }
    

}
