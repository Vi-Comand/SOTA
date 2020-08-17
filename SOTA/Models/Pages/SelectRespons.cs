using System.Collections.Generic;

namespace SOTA.Models
{

    public class SelectRespons
    {
        public List<Otvet> Otvets { get; }
        private readonly string _idSpec;
        private readonly string _nVar;
        private readonly SotaContext _db;
        public SelectRespons(SotaContext context)
        {

            _db = context;
            Otvets = new List<Otvet>();

        }

    }

}
