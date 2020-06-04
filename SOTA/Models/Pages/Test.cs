using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{

    public class Test
    {
        public List<ZadanNaProsmotr> Zadans { get; }
        private readonly int _idSpec;
        private readonly int _nVar;
        private readonly SotaContext _db;
     
    }
    public class OtvTest
    {
        int idOtv { get; set; }
        int Text { get; set; }

    }


}
