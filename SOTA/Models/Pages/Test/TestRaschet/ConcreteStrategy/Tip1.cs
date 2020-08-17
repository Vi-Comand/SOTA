using SOTA.Models.Pages.TestRaschet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Test.TestRaschet.ConcreteStrategy
{
    public class Tip1 : Strategy
    {


        public override List<AnswerUserRascheta> GetRaschet(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD)
        {
            if (OtvVBD != null)
                foreach (var row in OtvUsr)
                {
                    try
                    {
                        if (row.Ball == 0)
                            row.Ball = OtvVBD.FirstOrDefault(x => x.IdZadan == row.Otvet.IdZadan && x.Text == row.Otvet.TextOtv)
                                .Ball;
                        else
                        {
                            row.Ball = OtvVBD.FirstOrDefault(x =>
                                x.IdZadan == row.Otvet.IdZadan && x.Text == row.Otvet.TextOtv) != null
                                ? row.Ball
                                : 0;

                        }
                    }
                    catch
                    {
                        row.Ball = 0;
                    }
                }
            return OtvUsr;

        }
    }
}
    
    

