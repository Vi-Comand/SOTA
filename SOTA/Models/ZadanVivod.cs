using System;
using System.Collections.Generic;

namespace SOTA.Models
{
    public class ZadanVivod
    {
        public Zadanie Zadan { get; set; }

        public List<Otvet> Otv { get; set; }
    }
}