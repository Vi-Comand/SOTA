using System;
using System.Collections.Generic;

namespace SOTA.Models
{
    public class SpecifikacAddModel
    {
        public Specific Spec { get; set; }

        public List<Predm> Predms { get; set; }
        public List<TipSpec> TipSpecs { get; set; }
    }
}