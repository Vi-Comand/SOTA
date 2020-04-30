using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SOTA.Models
{
    public class SpecificSKolZad : Specific
    {
        public int KolZad;
        public SpecificSKolZad(SotaContext db) : base()
        {

            KolZad = !db.Zadanie.Any(x => x.Variant == 1 && x.IdSpec == Id) ? 0 : db.Zadanie.Count(x => x.Variant == 1 && x.IdSpec == Id);
        }




    }
}
