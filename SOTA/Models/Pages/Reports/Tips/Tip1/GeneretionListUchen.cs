using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Reports
{
    public class GeneretionListUchen
    {
        SotaContext db; 
   
       
         List<RowProtokol> UsrBalls;
        List<double> ProcVipZad;
        public GeneretionListUchen(List<RowProtokol> UserBalls, SotaContext context)
        {
            db = context;
            UsrBalls = UserBalls;
   
          
           
        }
        public List<double> GetProcVip()
        {
         
            return ProcVipZad;
        }
        public List<RowProtokol> GetTables()
        {
            GetListUchen();
            RaschetBalls();
            return UsrBalls;
        }


        private void RaschetBalls()
        {
            if(UsrBalls!=null)
            {
                double kolZad = UsrBalls[1].Balls.Count;
                foreach(var row in UsrBalls)
                {
                    row.SumBall = row.Balls.Where(x => x != -1).Sum();
                    row.ProcVipUch = row.Balls.Where(x => x != -1 && x != 0).Count() / kolZad * 100;
                }
                
                ProcVipZad = new List<double>();
                double a = UsrBalls.Count;
                for (int i=1;i<=kolZad;i++)
                {
                   
                    double b = UsrBalls.Select(x => x.Balls[i - 1]).Where(x => x != 0 && x != -1).Count();
                    double Proc =  b/a  * 100;
                   ProcVipZad.Add(Proc);
                }
               
            }

        }



            private void GetListUchen()
        {
            UsrBalls = (from Ball in UsrBalls

                        join US in db.Users on Ball.Id equals US.Id                      
                        join MO in db.Mo on US.IdMo equals MO.Id 
                        join OO in db.Oo on US.IdOo equals OO.Id
                        join Klass in db.Klass on US.IdKlass equals Klass.Id 
                        join Var in db.VariantUser on US.Id equals Var.IdUser 

                        select new RowProtokol
                        {
                            Id = US.Id,
                            FIO = (US.F!=null ? US.F+" " : "") + " " + (US.I!=null &&US.I.Any() ? US.I.Substring(0, 1) + "." : "") + (US.O != null && US.O.Any()  ? US.O.Substring(0, 1) + ".":""),
                             MO = MO.Name,
                             OO = OO.Name,
                             Klass = Klass.KlassNom + Klass.Kod,
                             Var = Var.Variant,
                             Balls = Ball.Balls

                         }).ToList();
        }
    }
}
