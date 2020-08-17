

using SOTA.Models.Pages.TestRaschet;
using System.Collections.Generic;

namespace SOTA.Models.Pages.Test.TestRaschet
{
    public class ContextStrategy
    {
        Strategy strategy;
        public ContextStrategy(Strategy strategy)
        {
            this.strategy = strategy;
        }
        public List<AnswerUserRascheta> GetRaschet(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD)
        {
           var list= strategy.GetRaschet(OtvUsr, OtvVBD);
            return list;
        }
    }
}
