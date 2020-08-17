using SOTA.Models.Pages.TestRaschet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.Test.TestRaschet
{
    public abstract class Strategy
    {
        public abstract List<AnswerUserRascheta> GetRaschet(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD);
    }
}
