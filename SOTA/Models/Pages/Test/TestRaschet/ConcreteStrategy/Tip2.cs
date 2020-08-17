using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOTA.Models.Pages.TestRaschet;

namespace SOTA.Models.Pages.Test.TestRaschet.ConcreteStrategy
{
    public class Tip2:Strategy
    {
        public override List<AnswerUserRascheta> GetRaschet(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD)
        {
            if (OtvVBD != null)
                foreach (var row in OtvUsr)
                {
                    List<int> idChecks = RazdelenieStrokiChecks(';', row.Otvet.TextOtv);
                    List<Otvet> sovpad = OtvVBD.Where(x => idChecks.Contains(x.Id)).ToList();
                    if (row.Ball == 0)
                    {

                        foreach (var str in sovpad)
                        {
                            row.Ball += str.Ball;
                        }


                    }
                    else
                    {
                        if (sovpad.Count != 0)
                        {
                            int kolVerno = OtvVBD.Count(x => x.IdZadan == sovpad[0].IdZadan);
                            if (sovpad.Count != kolVerno)
                            {
                                row.Ball = 0;
                            }
                        }
                        else
                        {
                            row.Ball = 0;
                        }
                    }
                }
            return OtvUsr;
        }
        private List<int> RazdelenieStrokiChecks(char Razdelitel, string Stroka)
        {
            List<int> idChecks = new List<int>();
            string[] words = Stroka.Split(new char[] { Razdelitel });
            if (words.Length == 1)
            {

                idChecks.Add(Convert.ToInt32(words[0]));
            }
            else
                for (int i = 0; i < words.Length - 1; i++)
                {
                    idChecks.Add(item: Convert.ToInt32(words[i]));
                }

            return idChecks;
        }
    }
}
