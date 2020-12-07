using System;
using System.Collections.Generic;

namespace SOTA.Models
{
    public class SpecifikacRedactModel
    {
        public Specific Spec { get; set; }
        public int KolVar { get; set; }
        public int KolZad { get; set; }
        public List<Predm> Predms { get; set; }
        public List<TipSpec> TipSpecs { get; set; }
        public List<Zadanie> Zadanies { get; set; }
        public List<Kriterocen> Kriterocens { get; set; }
    }
}