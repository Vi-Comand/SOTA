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
        List<double> SumVipZad;
        List<double> ProcVipZad;
        int idRabota;
        public GeneretionListUchen(List<RowProtokol> UserBalls, SotaContext context, int _idRabota)
        {
            db = context;
            UsrBalls = UserBalls;

            idRabota = _idRabota;


        }
        public List<double> GetProcVip()
        {
            return ProcVipZad;
        }

        public List<double> GetSumVip()
        {
            return SumVipZad;
        }
        public List<RowProtokol> GetTables(int IdSpec)
        {
            GetListUchen();
            RaschetBalls(IdSpec);
            return UsrBalls;
        }


        private void RaschetBalls(int ISp)
        {
            if(UsrBalls!=null)
            {
                var ListMaxBall = db.StructSpec.Where(x=>x.IdSpec==ISp && x.Type==1).OrderBy(x=>x.Number).ToList();
                double kolMaxBall=0;
                foreach (var row in ListMaxBall)
                {
                    kolMaxBall = kolMaxBall + Convert.ToDouble( row.Text);
                }
                   
                double kolZad = db.Zadanie.Where(x=>x.IdSpec==ISp && x.Variant==1).Count();
                foreach(var row in UsrBalls)
                {
                    row.SumBall = row.Balls.Where(x => x != -1).Sum();
                    row.ProcVipUch = row.Balls.Where(x => x != -1 && x != 0).Sum() / kolMaxBall * 100;
                }
                
                ProcVipZad = new List<double>();
                SumVipZad = new List<double>();
                double a = UsrBalls.Count;
                foreach(var lball in ListMaxBall)
                {
                    double b = UsrBalls.Select(x => x.Balls[lball.Number - 1]).Where(x => x != 0 && x != -1).Sum();
                    double Proc = b / a/Convert.ToDouble(lball.Text) ;
                    ProcVipZad.Add(Proc);
                }

                for (int i=1;i<=kolZad;i++)
                {
                   
                    double b = UsrBalls.Select(x => x.Balls[i - 1]).Where(x => x != 0 && x != -1).Sum();
                    double Sum =  b/a  ;
                   SumVipZad.Add(Sum);
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
                        join Var in db.VariantUser.Where(x=>x.IdRabota==idRabota) on US.Id equals Var.IdUser 

                        select new RowProtokol
                        {
                            Id = US.Id,
                            FIO = (US.F!=null ? US.F+" " : "") + " " + (US.I!=null &&US.I.Any() ? US.I.Substring(0, 1) + "." : "") + (US.O != null && US.O.Any()  ? US.O.Substring(0, 1) + ".":""),
                             MO = MO.Name,
                             OO = OO.Name,
                             Klass = Klass.Kod,
                             Var = Var.Variant,
                             Balls = Ball.Balls

                         }).ToList();
        }
    }
}
